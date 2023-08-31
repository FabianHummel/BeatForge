using System.Collections.ObjectModel;
using BeatForgeClient.Extensions;
using BeatForgeClient.Infrastructure;
using BeatForgeClient.Models;
using ReactiveUI;

namespace BeatForgeClient.ViewModels;

public class ChannelsViewModel : ViewModelBase
{
    private ChannelDto? _selectedChannel;
    public MainWindowViewModel MainVm { get; }
    public BeatForgeContext Db => MainVm.Db;

    public ChannelsViewModel(MainWindowViewModel mainVm)
    {
        MainVm = mainVm;

        MainVm.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(MainVm.Song))
            {
                LoadSongChannels();
            }
        };
    }

    public string NewChannelName { get; set; } = string.Empty;

    public ObservableCollection<ChannelDto> SongChannels { get; } = new();

    public ChannelDto? SelectedChannel
    {
        get => _selectedChannel;
        set => this.RaiseAndSetIfChanged(ref _selectedChannel, value);
    }

    public void LoadSongChannels()
    {
        if (MainVm.Song == null) return;
        Logger.Task("Loading channels... ");
        SongChannels.ReplaceAll(MainVm.Song.Channels);
        Logger.Complete($"({SongChannels.Count} channels loaded).");
    }

    public void NewChannel()
    {
        if (MainVm.Song == null) return;
        Logger.Task($"Creating new channel... ");

        var channelDto = new ChannelDto
        {
            Name = NewChannelName,
            Song = MainVm.Song,
            Volume = 0.5f,
            Instrument = Instrument.Square,
            Notes = new()
        };

        SongChannels.Add(channelDto);
        NewChannelName = string.Empty;

        this.RaisePropertyChanged(nameof(NewChannelName));
        this.RaisePropertyChanged(nameof(SongChannels));
        Logger.Complete($"{channelDto.Name} created.");
    }

    public void SaveSongChannels()
    {
        if (MainVm.Song == null) return;
        Logger.Task("Saving channels... ");
        MainVm.Song.Channels.ReplaceAll(SongChannels);
        Logger.Complete("Channels saved.");
    }
}
