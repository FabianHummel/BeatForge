using System;
using BeatForgeClient.Extensions;
using BeatForgeClient.Infrastructure;
using BeatForgeClient.Models;
using ReactiveUI;

namespace BeatForgeClient.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public BeatForgeContext Db { get; }

    public TitlebarViewModel TitlebarViewModel { get; }
    public SettingsViewModel SettingsViewModel { get; }
    public ChannelsViewModel ChannelsViewModel { get; }
    public ContentViewModel ContentViewModel { get; }

    public MainWindowViewModel()
    {
        Db = new BeatForgeContext();
        Db.SavedChanges += (_, args) =>
        {
            Logger.Task("Saving changes... ");
            Logger.Complete($"({args.EntitiesSavedCount} entities saved).");
        };
        TitlebarViewModel = new TitlebarViewModel(this);
        SettingsViewModel = new SettingsViewModel(this);
        ChannelsViewModel = new ChannelsViewModel(this);
        ContentViewModel = new ContentViewModel(this);
    }
    
    private SongDto? _song;

    public SongDto? Song
    {
        get => _song;
        set => this.RaiseAndSetIfChanged(ref _song, value);
    }
}