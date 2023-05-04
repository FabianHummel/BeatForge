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
	public MainWindowViewModel MainWindowViewModel { get; }
	public BeatForgeContext Db => MainWindowViewModel.Db;

	public ChannelsViewModel(MainWindowViewModel mainWindowViewModel)
	{
		MainWindowViewModel = mainWindowViewModel;

		MainWindowViewModel.PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName == nameof(MainWindowViewModel.Song))
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
		if (MainWindowViewModel.Song == null) return;
		Console.Write("Loading channels... ");

		var channels = from c in Db.Channels
			where c.Song.Id == MainWindowViewModel.Song.Id
			select c;
		
		SongChannels.ReplaceAll(Program.Mapper
			.ProjectTo<ChannelDto>(channels, membersToExpand: dto => new
			{
				dto.Instrument,
				dto.Song
			}));
		
		Console.WriteLine($"done. {SongChannels.Count} channels loaded.");
	}

	public void NewChannel()
	{
		if (MainWindowViewModel.Song == null) return;
		Console.Write("Creating new channel... ");
		
		var channelDto = new ChannelDto
		{
			Name = NewChannelName,
			Song = MainWindowViewModel.Song,
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
		Console.WriteLine("done.");
	}

	public void SaveChannels()
	{
		if (MainWindowViewModel.Song is null) return;
		Console.WriteLine("Saving channels... ");
		
		MainWindowViewModel.Song.Channels.ReplaceAll(
			SongChannels);
		
		
		
		// MainWindowViewModel.TitlebarViewModel.SaveSong();

		try
		{
			Console.Write("Searching for existing channels... ");
			var channelsDb = Db.Channels.Where(c =>
				c.Song.Id == MainWindowViewModel.Song.Id);
			Db.Channels.RemoveRange(channelsDb);
			Console.WriteLine($"done. {channelsDb.Count()} channels removed.");
			
			Console.Write("Searching for song... ");
			var song = Db.Songs.FirstOrDefault(s =>
				s.Id == MainWindowViewModel.Song.Id);
			if (song is not null)
			{
				Db.Songs.Remove(song);
				Console.WriteLine("done. Song removed.");
			}
			else
			{
				Console.WriteLine("done. Song not found.");
			}
		
			Console.Write("Mapping channels... ");
			var channels = Program.Mapper.Map<IEnumerable<Channel>>(SongChannels);
			Db.Channels.AddRange(channels);
			Db.SaveChanges();
			Console.WriteLine("done.");
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
		}
	}
}