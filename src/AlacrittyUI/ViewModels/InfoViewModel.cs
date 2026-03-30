using System.Reflection;
using AlacrittyUI.Resources;
using AlacrittyUI.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AlacrittyUI.ViewModels;

public partial class InfoViewModel : ObservableObject
{
    private readonly AppSettingsService _appSettings;

    public string AppName => "AlacrittyUI";

    public string Version { get; } = GetVersion();

    private static string GetVersion()
    {
        var ver = Assembly.GetExecutingAssembly().GetName().Version;
        return ver != null ? $"{ver.Major}.{ver.Minor}.{ver.Build}" : "0.1.0";
    }

    public string Developer => "Codixio";
    public string Website => "https://codixio.com";
    public string RepoUrl => "https://github.com/codixio/AlacrittyUI";
    public string AlacrittyRepoUrl => "https://github.com/alacritty/alacritty";
    public string AlacrittyVersion => "0.15+";
    public string Year => "2026";

    [ObservableProperty]
    private string _selectedLanguage;

    public string[] LanguageOptions { get; } = ["English", "Deutsch"];

    public string RestartHint => Strings.InfoRestartHint;

    public InfoViewModel(AppSettingsService appSettings)
    {
        _appSettings = appSettings;
        _selectedLanguage = _appSettings.Settings.Language == "de" ? "Deutsch" : "English";
    }

    partial void OnSelectedLanguageChanged(string value)
    {
        var code = value == "Deutsch" ? "de" : "en";
        _appSettings.Settings.Language = code;
        _appSettings.Save();
    }
}
