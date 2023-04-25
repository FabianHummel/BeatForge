using BeatForgeClient.Infrastructure;

namespace BeatForgeClient.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly BeatForgeContext _db = new();
    
    public TitlebarViewModel TitlebarViewModel { get; }
    public SettingsViewModel SettingsViewModel { get; }
    public ChannelsViewModel ChannelsViewModel { get; }
    public ContentViewModel ContentViewModel { get; }

    public MainWindowViewModel()
    {
        _db.Database.EnsureCreated();
        TitlebarViewModel = new TitlebarViewModel();
        SettingsViewModel = new SettingsViewModel();
        ChannelsViewModel = new ChannelsViewModel();
        ContentViewModel = new ContentViewModel();
    }
}