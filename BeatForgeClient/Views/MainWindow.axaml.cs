using Avalonia.Controls;
using Avalonia.Input;
using BeatForgeClient.ViewModels;

namespace BeatForgeClient.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private MainWindowViewModel MainWindowViewModel => (DataContext as MainWindowViewModel)!;
    
    private void InputElement_OnKeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Space:
                MainWindowViewModel.SettingsViewModel.TogglePlaySong();
                break;
            
            case Key.S:
                MainWindowViewModel.ChannelsViewModel.ToggleSingleCurrent();
                break;
            
            case Key.M:
                MainWindowViewModel.ChannelsViewModel.ToggleMuteCurrent();
                break;
            
            case Key.OemMinus:
                if ((e.KeyModifiers & KeyModifiers.Control) != 0)
                {
                    MainWindowViewModel.ChannelsViewModel.MoveNotes(-12);
                }
                else
                {
                    MainWindowViewModel.ChannelsViewModel.MoveNotes(-1);
                }
                break;
            
            case Key.OemPlus:
                if ((e.KeyModifiers & KeyModifiers.Control) != 0)
                {
                    MainWindowViewModel.ChannelsViewModel.MoveNotes(+12);
                }
                else
                {
                    MainWindowViewModel.ChannelsViewModel.MoveNotes(+1);
                }
                break;
        }
    }
}