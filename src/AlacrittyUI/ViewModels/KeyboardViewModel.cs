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

    public string[] ActionOptions => KeyBinding.ActionOptions;
    public string[] ModOptions => KeyBinding.ModOptions;
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
