namespace BeatForgeClient.ViewModels;

public class ContentViewModel : ViewModelBase
{
    public MainWindowViewModel MainWindowViewModel { get; }
    
    public ContentViewModel(MainWindowViewModel mainWindowViewModel)
    {
        MainWindowViewModel = mainWindowViewModel;
    }
}