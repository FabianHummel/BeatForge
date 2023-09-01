using BeatForgeClient.Extensions;
using BeatForgeClient.Infrastructure;

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
}