using System;
using BeatForgeClient.Infrastructure;
using ReactiveUI;

namespace BeatForgeClient.ViewModels;

public class SettingsViewModel : ViewModelBase
{
    public MainWindowViewModel MainWindowViewModel { get; }
    public BeatForgeContext Db => MainWindowViewModel.Db;
    
    public SettingsViewModel(MainWindowViewModel mainWindowViewModel)
    {
        MainWindowViewModel = mainWindowViewModel;
        
        MainWindowViewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(MainWindowViewModel.Song))
            {
                this.RaisePropertyChanged(nameof(SongTitle));
            }
        };
    }

    public string SongTitle
    {
        get => MainWindowViewModel.Song?.Name ?? "No Song Selected";
        set
        {
            if (MainWindowViewModel.Song != null)
            {
                MainWindowViewModel.Song.Name = value;
                this.RaisePropertyChanged(nameof(MainWindowViewModel.TitlebarViewModel.StoredSongs));
            }
        }
    }
}