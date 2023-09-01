using System;
using System.Collections.ObjectModel;
using BeatForgeClient.Extensions;
using BeatForgeClient.Models;
using ReactiveUI;

namespace BeatForgeClient.ViewModels;

public class ChannelsViewModel : ViewModelBase
{
    private ChannelDto? _selectedChannel;
    public MainWindowViewModel MainVm { get; }

    public ChannelsViewModel(MainWindowViewModel mainVm)
    {
        MainVm = mainVm;

        MainVm.TitlebarViewModel.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(MainVm.TitlebarViewModel.SelectedSong))
            {
                this.RaisePropertyChanged(nameof(SongChannels));
            }
        };
    }

    public string NewChannelName { get; set; } = string.Empty;

    public ObservableCollection<ChannelDto> SongChannels => MainVm.TitlebarViewModel.SelectedSong!.Channels;

    public ChannelDto? SelectedChannel
    {
        get => _selectedChannel;
        set => this.RaiseAndSetIfChanged(ref _selectedChannel, value);
    }

    public void NewChannel()
    {
        if (MainVm.TitlebarViewModel.SelectedSong is null) return;
        Logger.Task($"Creating new channel... ");

        var channelDto = new ChannelDto
        {
            Name = NewChannelName,
            Song = MainVm.TitlebarViewModel.SelectedSong,
            Volume = 0.5f,
            Instrument = Instrument.Square
        };

        SongChannels.Add(channelDto);
        NewChannelName = string.Empty;

        this.RaisePropertyChanged(nameof(NewChannelName));
        this.RaisePropertyChanged(nameof(SongChannels));
        Logger.Complete($"{channelDto.Name} created.");
    }

    public void DeleteSelectedChannel()
    {
        if (SelectedChannel is null) return;
        SongChannels.Remove(SelectedChannel);
    }

    public void ToggleSingleCurrent()
    {
        if (SelectedChannel is null) return;
        
        var oneOrMoreMuted = false;
        SelectedChannel.Muted = false;
        foreach (var channel in SongChannels)
        {
            if (channel != SelectedChannel && !channel.Muted)
            {
                channel.Muted = true;
                oneOrMoreMuted = true;
            }
        }

        if (!oneOrMoreMuted)
        {
            foreach (var channel in SongChannels)
            {
                channel.Muted = false;
            }
        }
        
        this.RaisePropertyChanged(nameof(SongChannels));
    }

    public void ToggleMuteCurrent()
    {
        if (SelectedChannel is null) return;
        SelectedChannel.Muted = !SelectedChannel.Muted;
        this.RaisePropertyChanged(nameof(SongChannels));
    }

    public void MoveNotes(int amount)
    {
        if (SelectedChannel is null) return;
        foreach (var note in SelectedChannel.Notes)
        {
            note.Pitch += amount;
        }
        
        MainVm.ContentViewModel.RaisePropertyChanged(nameof(MainVm.ContentViewModel.ChannelNotes));
    }
}
