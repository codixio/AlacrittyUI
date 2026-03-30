using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using AlacrittyUI.Models;
using AlacrittyUI.Resources;
using AlacrittyUI.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Serilog;

namespace AlacrittyUI.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private static readonly ILogger Logger = Log.ForContext<MainWindowViewModel>();

    private readonly ConfigDiscoveryService _discovery;
    private readonly ConfigReaderService _reader;
    private readonly ConfigWriterService _writer;
    private readonly AppSettingsService _appSettings;
    private readonly BackupService _backupService;

    public string WindowTitle { get; } = $"AlacrittyUI v{GetVersion()}";

    [ObservableProperty]
    private string _configPath = string.Empty;

    [ObservableProperty]
    private string _statusText = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsTab0))]
    [NotifyPropertyChangedFor(nameof(IsTab1))]
    [NotifyPropertyChangedFor(nameof(IsTab2))]
    [NotifyPropertyChangedFor(nameof(IsTab3))]
    [NotifyPropertyChangedFor(nameof(IsTab4))]
    [NotifyPropertyChangedFor(nameof(IsTab5))]
    [NotifyPropertyChangedFor(nameof(IsTab6))]
    [NotifyPropertyChangedFor(nameof(IsTab7))]
    [NotifyPropertyChangedFor(nameof(IsTab8))]
    [NotifyPropertyChangedFor(nameof(IsTab9))]
    private int _selectedTabIndex;

    public bool IsTab0 => SelectedTabIndex == 0;
    public bool IsTab1 => SelectedTabIndex == 1;
    public bool IsTab2 => SelectedTabIndex == 2;
    public bool IsTab3 => SelectedTabIndex == 3;
    public bool IsTab4 => SelectedTabIndex == 4;
    public bool IsTab5 => SelectedTabIndex == 5;
    public bool IsTab6 => SelectedTabIndex == 6;
    public bool IsTab7 => SelectedTabIndex == 7;
    public bool IsTab8 => SelectedTabIndex == 8;
    public bool IsTab9 => SelectedTabIndex == 9;

    [ObservableProperty]
    private bool _hasConfig;

    [ObservableProperty]
    private bool _isDirty;

    [ObservableProperty]
    private bool _showResetDialog;

    [ObservableProperty]
    private bool _showUnsavedDialog;

    [ObservableProperty]
    private bool _showRevertDialog;

    [ObservableProperty]
    private BackupInfo? _selectedBackup;

    [ObservableProperty]
    private string _backupPreview = string.Empty;

    [ObservableProperty]
    private ObservableCollection<BackupInfo> _backups = [];

    public ColorEditorViewModel ColorEditor { get; }
    public FontViewModel Font { get; }
    public WindowViewModel Window { get; }
    public CursorViewModel Cursor { get; }
    public TerminalViewModel Terminal { get; }
    public HintsViewModel Hints { get; }
    public KeyboardViewModel Keyboard { get; }
    public DebugViewModel Debug { get; }
    public ThemeManagerViewModel ThemeManager { get; }
    public InfoViewModel Info { get; }

    private AlacrittyConfig? _config;
    private bool _suppressDirtyTracking;
    private Action? _pendingAction;
    private readonly List<(System.Collections.Specialized.INotifyCollectionChanged Collection, System.Collections.Specialized.NotifyCollectionChangedEventHandler Handler)> _collectionSubscriptions = [];

    public MainWindowViewModel(
        ConfigDiscoveryService discovery,
        ConfigReaderService reader,
        ConfigWriterService writer,
        ThemeService themeService,
        AppSettingsService appSettings,
        BackupService backupService)
    {
        _discovery = discovery;
        _reader = reader;
        _writer = writer;
        _appSettings = appSettings;
        _backupService = backupService;

        ColorEditor = new ColorEditorViewModel();
        Font = new FontViewModel();
        Window = new WindowViewModel();
        Cursor = new CursorViewModel();
        Terminal = new TerminalViewModel();
        Hints = new HintsViewModel();
        Keyboard = new KeyboardViewModel();
        Debug = new DebugViewModel();
        ThemeManager = new ThemeManagerViewModel(themeService, this);
        Info = new InfoViewModel(appSettings);

        SubscribeToChanges();
        InitializeConfig();
    }

    private void SubscribeToChanges()
    {
        ObservableObject[] tracked = [ColorEditor, Font, Window, Cursor, Terminal, Hints, Keyboard, Debug];
        foreach (var vm in tracked)
            vm.PropertyChanged += OnChildPropertyChanged;

        // track color entry changes within collections
        ColorEditor.PropertyChanged += OnChildPropertyChanged;
        SubscribeToColorCollections();
    }

    private void SubscribeToColorCollections()
    {
        // unsubscribe existing handlers to prevent leaks on reload
        UnsubscribeFromColorCollections();

        var collections = GetColorCollections();

        foreach (var collection in collections)
        {
            System.Collections.Specialized.NotifyCollectionChangedEventHandler handler = (_, args) =>
            {
                if (args.NewItems != null)
                {
                    foreach (ColorEntry entry in args.NewItems)
                        entry.PropertyChanged += OnChildPropertyChanged;
                }
            };
            collection.CollectionChanged += handler;
            _collectionSubscriptions.Add((collection, handler));

            foreach (var entry in collection)
                entry.PropertyChanged += OnChildPropertyChanged;
        }
    }

    private void UnsubscribeFromColorCollections()
    {
        foreach (var (collection, handler) in _collectionSubscriptions)
            collection.CollectionChanged -= handler;
        _collectionSubscriptions.Clear();

        foreach (var collection in GetColorCollections())
            foreach (var entry in collection)
                entry.PropertyChanged -= OnChildPropertyChanged;
    }

    private System.Collections.ObjectModel.ObservableCollection<ColorEntry>[] GetColorCollections() =>
    [
        ColorEditor.PrimaryColors, ColorEditor.NormalColors, ColorEditor.BrightColors,
        ColorEditor.CursorColors, ColorEditor.SelectionColors, ColorEditor.SearchColors,
        ColorEditor.FooterColors, ColorEditor.ViModeCursorColors, ColorEditor.HintsColors,
        ColorEditor.LineIndicatorColors, ColorEditor.DimColors
    ];

    private void OnChildPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (!_suppressDirtyTracking)
            IsDirty = true;
    }

    private void LoadAllViewModels(AlacrittyConfig config)
    {
        _suppressDirtyTracking = true;
        try
        {
            ColorEditor.LoadFromPalette(config.Colors);
            Font.LoadFrom(config.Font);
            Window.LoadFrom(config.Window);
            Cursor.LoadFrom(config.Cursor);
            Terminal.LoadFrom(config.Terminal);
            Hints.LoadFrom(config.Hints);
            Keyboard.LoadFrom(config.Keyboard);
            Debug.LoadFrom(config.Debug);

            // re-subscribe to color entries after reload
            SubscribeToColorCollections();
        }
        finally
        {
            _suppressDirtyTracking = false;
            IsDirty = false;
        }
    }

    private void InitializeConfig()
    {
        var lastPath = _appSettings.Settings.LastConfigPath;
        var path = (lastPath != null && File.Exists(lastPath))
            ? lastPath
            : _discovery.FindConfigPath();

        if (path != null)
        {
            LoadConfigFromPath(path);
        }
        else
        {
            // no config found — use default path so the user can save
            ConfigPath = ConfigDiscoveryService.GetDefaultConfigPath();
            StatusText = Strings.StatusNoConfig;
            HasConfig = true;
            var defaults = new AlacrittyConfig();
            _config = defaults;
            LoadAllViewModels(defaults);
        }
    }

    public void LoadConfigFromPath(string path)
    {
        try
        {
            ConfigPath = path;
            _config = _reader.ReadFromFile(path);
            LoadAllViewModels(_config);
            HasConfig = true;
            StatusText = Strings.StatusLoaded;
            Logger.Information("Config loaded from {Path}", path);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Failed to load config from {Path}", path);
            StatusText = Strings.StatusLoadError;
            HasConfig = false;
        }
    }

    /// <summary>
    /// Checks for unsaved changes before executing an action.
    /// If dirty, shows the unsaved dialog and defers the action.
    /// Returns true if the action was deferred (dialog shown).
    /// </summary>
    public bool GuardUnsavedChanges(Action action)
    {
        if (!IsDirty)
        {
            action();
            return false;
        }

        _pendingAction = action;
        ShowUnsavedDialog = true;
        return true;
    }

    [RelayCommand]
    private void Save()
    {
        if (_config == null || string.IsNullOrEmpty(ConfigPath)) return;

        if (HasValidationErrors())
        {
            StatusText = Strings.StatusValidationError;
            return;
        }

        try
        {
            ColorEditor.ApplyToPalette(_config.Colors);
            Font.ApplyTo(_config.Font);
            Window.ApplyTo(_config.Window);
            Cursor.ApplyTo(_config.Cursor);
            Terminal.ApplyTo(_config.Terminal);
            Hints.ApplyTo(_config.Hints);
            Keyboard.ApplyTo(_config.Keyboard);
            Debug.ApplyTo(_config.Debug);
            _writer.WriteConfig(ConfigPath, _config);
            IsDirty = false;
            StatusText = Strings.StatusSaved;
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Failed to save config to {Path}", ConfigPath);
            StatusText = Strings.StatusSaveError;
        }
    }

    // unsaved dialog: save, then run pending action
    [RelayCommand]
    private void UnsavedSave()
    {
        ShowUnsavedDialog = false;
        Save();
        var action = _pendingAction;
        _pendingAction = null;
        action?.Invoke();
    }

    // unsaved dialog: discard changes, run pending action
    [RelayCommand]
    private void UnsavedDiscard()
    {
        ShowUnsavedDialog = false;
        IsDirty = false;
        var action = _pendingAction;
        _pendingAction = null;
        action?.Invoke();
    }

    // unsaved dialog: cancel, go back
    [RelayCommand]
    private void UnsavedCancel()
    {
        ShowUnsavedDialog = false;
        _pendingAction = null;
    }

    public void ApplyPalette(ColorPalette palette)
    {
        _config ??= new AlacrittyConfig();
        _config.Colors = palette;

        _suppressDirtyTracking = true;
        ColorEditor.LoadFromPalette(palette);
        SubscribeToColorCollections();
        _suppressDirtyTracking = false;

        IsDirty = true;
    }

    public ColorPalette? GetCurrentPalette()
    {
        if (_config == null) return null;
        ColorEditor.ApplyToPalette(_config.Colors);
        return _config.Colors;
    }

    [RelayCommand]
    private void RequestReset()
    {
        ShowResetDialog = true;
    }

    [RelayCommand]
    private void ConfirmReset()
    {
        ShowResetDialog = false;
        var defaults = new AlacrittyConfig();
        _config = defaults;
        LoadAllViewModels(defaults);
        IsDirty = true;
        StatusText = Strings.StatusReset;
        Logger.Information("All settings reset to defaults");
    }

    [RelayCommand]
    private void CancelReset()
    {
        ShowResetDialog = false;
    }

    partial void OnSelectedBackupChanged(BackupInfo? value)
    {
        if (value != null)
            BackupPreview = _backupService.ReadBackupContent(value.FilePath);
        else
            BackupPreview = string.Empty;
    }

    [RelayCommand]
    private void Revert()
    {
        if (string.IsNullOrEmpty(ConfigPath)) return;
        Backups = new ObservableCollection<BackupInfo>(_backupService.GetBackups(ConfigPath));
        SelectedBackup = null;
        BackupPreview = string.Empty;
        ShowRevertDialog = true;
    }

    [RelayCommand]
    private void ConfirmRevert()
    {
        if (SelectedBackup == null || string.IsNullOrEmpty(ConfigPath)) return;

        try
        {
            _backupService.RestoreBackup(SelectedBackup.FilePath, ConfigPath);
            ShowRevertDialog = false;
            LoadConfigFromPath(ConfigPath);
            StatusText = Strings.StatusReverted;
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Failed to restore backup");
            StatusText = Strings.StatusRevertError;
        }
    }

    [RelayCommand]
    private void CancelRevert()
    {
        ShowRevertDialog = false;
    }

    [RelayCommand]
    private void OpenConfigInEditor()
    {
        if (string.IsNullOrEmpty(ConfigPath) || !File.Exists(ConfigPath)) return;

        try
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Process.Start(new ProcessStartInfo(ConfigPath) { UseShellExecute = true });
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                Process.Start("open", ConfigPath);
            else
                Process.Start("xdg-open", ConfigPath);
        }
        catch (Exception ex)
        {
            Logger.Warning(ex, "Failed to open config file in editor");
        }
    }

    /// <summary>
    /// Called by MainWindow before closing. Returns true to cancel close.
    /// </summary>
    public bool OnClosing()
    {
        if (!IsDirty) return false;

        // defer close — show dialog, user decides
        _pendingAction = () => { /* window will handle close after dialog */ };
        ShowUnsavedDialog = true;
        return true;
    }

    private bool HasValidationErrors()
    {
        foreach (var collection in GetColorCollections())
            foreach (var entry in collection)
                if (entry.HasError)
                    return true;

        foreach (var rule in Hints.Rules)
            if (rule.RegexHasError)
                return true;

        return false;
    }

    private static string GetVersion()
    {
        var ver = Assembly.GetExecutingAssembly().GetName().Version;
        return ver != null ? $"{ver.Major}.{ver.Minor}.{ver.Build}" : "0.1.0";
    }
}
