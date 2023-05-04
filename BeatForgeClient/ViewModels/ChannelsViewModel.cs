using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AutoMapper.QueryableExtensions;
using BeatForgeClient.Extensions;
using BeatForgeClient.Infrastructure;
using ReactiveUI;

namespace BeatForgeClient.ViewModels;

public class ChannelsViewModel : ViewModelBase
{
	public MainWindowViewModel MainVm { get; }
	public BeatForgeContext Db => MainVm.Db;

	public ChannelsViewModel(MainWindowViewModel mainVm)
	{
		MainVm = mainVm;

		MainVm.PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName == nameof(MainVm.Song))
			{
				LoadSongChannels();
			}
		};
	}

	public string NewChannelName { get; set; } = string.Empty;

	public ObservableCollection<ChannelDto> SongChannels { get; } = new();

	public Channel? SelectedChannel { get; set; }

	public void LoadSongChannels()
	{
		if (MainVm.Song == null) return;
		Console.Write("\nLoading channels... ");

		SongChannels.ReplaceAll(MainVm.Song.Channels);

		Console.Write($"done ({SongChannels.Count} channels loaded).");
	}

	public void NewChannel()
	{
		if (MainVm.Song == null) return;
		Console.Write("\nCreating new channel... ");
		
		var channelDto = new ChannelDto
		{
			Name = NewChannelName,
			Song = MainVm.Song,
			Volume = 50.0,
		};

		channelDto.Instrument = new()
		{
			Name = "Instrument",
			Channel = channelDto
		};

		SongChannels.Add(channelDto);
		NewChannelName = string.Empty;

		this.RaisePropertyChanged(nameof(NewChannelName));
		this.RaisePropertyChanged(nameof(SongChannels));
		Console.Write("done.");
	}
}