using System.Collections.ObjectModel;
using AlacrittyUI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AlacrittyUI.ViewModels;

public partial class HintRuleViewModel : ObservableObject
{
    [ObservableProperty] private string _regex = string.Empty;
    [ObservableProperty] private bool _hyperlinks;
    [ObservableProperty] private bool _postProcessing;
    [ObservableProperty] private bool _persist;
    [ObservableProperty] private string _action = string.Empty;
    [ObservableProperty] private string _command = string.Empty;
    [ObservableProperty] private string _bindingKey = string.Empty;
    [ObservableProperty] private string _bindingMods = string.Empty;
    [ObservableProperty] private bool _mouseEnabled = true;
    [ObservableProperty] private string _mouseMods = string.Empty;

    public string[] ActionOptions => HintRule.ActionOptions;
}

public partial class HintsViewModel : ObservableObject
{
    [ObservableProperty] private string _alphabet = "jfkdls;ahgurieowpq";
    [ObservableProperty] private HintRuleViewModel? _selectedRule;

    public ObservableCollection<HintRuleViewModel> Rules { get; } = [];

    public void LoadFrom(HintsConfig h)
    {
        Alphabet = h.Alphabet;
        Rules.Clear();
        foreach (var rule in h.Enabled)
        {
            Rules.Add(new HintRuleViewModel
            {
                Regex = rule.Regex,
                Hyperlinks = rule.Hyperlinks,
                PostProcessing = rule.PostProcessing,
                Persist = rule.Persist,
                Action = rule.Action,
                Command = rule.Command,
                BindingKey = rule.BindingKey,
                BindingMods = rule.BindingMods,
                MouseEnabled = rule.MouseEnabled,
                MouseMods = rule.MouseMods
            });
        }
    }

    public void ApplyTo(HintsConfig h)
    {
        h.Alphabet = Alphabet;
        h.Enabled.Clear();
        foreach (var vm in Rules)
        {
            h.Enabled.Add(new HintRule
            {
                Regex = vm.Regex,
                Hyperlinks = vm.Hyperlinks,
                PostProcessing = vm.PostProcessing,
                Persist = vm.Persist,
                Action = vm.Action,
                Command = vm.Command,
                BindingKey = vm.BindingKey,
                BindingMods = vm.BindingMods,
                MouseEnabled = vm.MouseEnabled,
                MouseMods = vm.MouseMods
            });
        }
    }

    [RelayCommand]
    private void AddRule()
    {
        var rule = new HintRuleViewModel();
        Rules.Add(rule);
        SelectedRule = rule;
    }

    [RelayCommand]
    private void RemoveRule()
    {
        if (SelectedRule == null) return;
        Rules.Remove(SelectedRule);
        SelectedRule = null;
    }
}
