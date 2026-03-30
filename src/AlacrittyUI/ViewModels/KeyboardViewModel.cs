using System.Collections.ObjectModel;
using AlacrittyUI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AlacrittyUI.ViewModels;

public partial class KeyBindingViewModel : ObservableObject
{
    [ObservableProperty] private string _key = string.Empty;
    [ObservableProperty] private string _mods = string.Empty;
    [ObservableProperty] private string _mode = string.Empty;
    [ObservableProperty] private string _action = string.Empty;
    [ObservableProperty] private string _command = string.Empty;
    [ObservableProperty] private string _chars = string.Empty;
    internal List<string> CommandArgs { get; set; } = [];

    public string[] ActionOptions => KeyBinding.ActionOptions;
    public string[] ModOptions => KeyBinding.ModOptions;
    public string[] KeyOptions => KeyBinding.CommonKeys;
    public string[] ModeOptions => KeyBinding.ModeOptions;

    [ObservableProperty] private bool _modControl;
    [ObservableProperty] private bool _modShift;
    [ObservableProperty] private bool _modAlt;
    [ObservableProperty] private bool _modSuper;

    private bool _suppressModSync;

    partial void OnModControlChanged(bool value) => SyncModsFromCheckboxes();
    partial void OnModShiftChanged(bool value) => SyncModsFromCheckboxes();
    partial void OnModAltChanged(bool value) => SyncModsFromCheckboxes();
    partial void OnModSuperChanged(bool value) => SyncModsFromCheckboxes();

    private void SyncModsFromCheckboxes()
    {
        if (_suppressModSync) return;
        var parts = new List<string>();
        if (ModControl) parts.Add("Control");
        if (ModShift) parts.Add("Shift");
        if (ModAlt) parts.Add("Alt");
        if (ModSuper) parts.Add("Super");
        _suppressModSync = true;
        Mods = string.Join("|", parts);
        _suppressModSync = false;
    }

    partial void OnModsChanged(string value)
    {
        if (_suppressModSync) return;
        _suppressModSync = true;
        var parts = value.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        ModControl = parts.Contains("Control");
        ModShift = parts.Contains("Shift");
        ModAlt = parts.Contains("Alt");
        ModSuper = parts.Contains("Super");
        _suppressModSync = false;
    }
}

public partial class KeyboardViewModel : ObservableObject
{
    [ObservableProperty] private KeyBindingViewModel? _selectedBinding;

    public ObservableCollection<KeyBindingViewModel> Bindings { get; } = [];

    public void LoadFrom(KeyboardConfig kb)
    {
        Bindings.Clear();
        foreach (var b in kb.Bindings)
        {
            Bindings.Add(new KeyBindingViewModel
            {
                Key = b.Key,
                Mods = b.Mods,
                Mode = b.Mode,
                Action = b.Action,
                Command = b.Command,
                CommandArgs = b.CommandArgs,
                Chars = b.Chars
            });
        }
    }

    public void ApplyTo(KeyboardConfig kb)
    {
        kb.Bindings.Clear();
        foreach (var vm in Bindings)
        {
            kb.Bindings.Add(new KeyBinding
            {
                Key = vm.Key,
                Mods = vm.Mods,
                Mode = vm.Mode,
                Action = vm.Action,
                Command = vm.Command,
                CommandArgs = vm.CommandArgs,
                Chars = vm.Chars
            });
        }
    }

    [RelayCommand]
    private void AddBinding()
    {
        var binding = new KeyBindingViewModel();
        Bindings.Add(binding);
        SelectedBinding = binding;
    }

    [RelayCommand]
    private void RemoveBinding()
    {
        if (SelectedBinding == null) return;
        Bindings.Remove(SelectedBinding);
        SelectedBinding = null;
    }
}
