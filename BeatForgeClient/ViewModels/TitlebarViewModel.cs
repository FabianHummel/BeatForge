using System;
using System.Collections.ObjectModel;
using System.Linq;
using AutoMapper.QueryableExtensions;
using BeatForgeClient.Extensions;
using BeatForgeClient.Infrastructure;
using BeatForgeClient.Models;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;

namespace BeatForgeClient.ViewModels;

public class TitlebarViewModel : ViewModelBase
{
    public MainWindowViewModel MainVm { get; }
    public BeatForgeContext Db => MainVm.Db;
    
    public TitlebarViewModel(MainWindowViewModel mainVm)
    {
        MainVm = mainVm;
        PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(SelectedSong))
            {
                LoadSelectedSong();
            }
        };
        
        LoadStoredSongs();
    }

    public string Title { get; set; } = "BeatForge";
    public string NewSongName { get; set; } = string.Empty;
    
    public ObservableCollection<SongDto> StoredSongs { get; } = new();
    private SongDto? _selectedSong;

    public SongDto? SelectedSong
    {
        get => _selectedSong;
        set => this.RaiseAndSetIfChanged(ref _selectedSong, value);
    }

    /// <summary>
    /// Loads all songs from the database and into the
    /// <see cref="StoredSongs"/> collection.
    /// Note that StoredSongs only holds values that are present in the
    /// "Songs" table, and does not include associated channels or preferences.
    /// See <see cref="LoadSelectedSong"/> for loading the full data.
    /// </summary>
    public void LoadStoredSongs()
    {
        Logger.Task("Loading stored songs... ");
        var songs = from s in Db.Songs 
            select new SongDto { Id = s.Id, Name = s.Name };
        StoredSongs.ReplaceAll(songs);
        Logger.Complete($"({StoredSongs.Count} songs loaded).");
    }

    /// <summary>
    /// Loads the full data for the selected song. This includes the
    /// channels and preferences associated with the song.
    /// </summary>
    public void LoadSelectedSong()
    {
        if (SelectedSong is null) return;
        Logger.Task($"Loading song \"{SelectedSong.Name}\"... ");
        
        var song = Db.Songs.FirstOrDefault(s => s.Id == SelectedSong.Id);
        if (song is null)
        {
            var songDto = StoredSongs.FirstOrDefault(s => s.Id == SelectedSong.Id);
            MainVm.Song = songDto;
        }
        else
        {
            var songDto = Program.Mapper.Map<SongDto>(song);
            MainVm.Song = songDto;
        }
        Logger.Complete("Song loaded.");
    }
    
    /// <summary>
    /// Creates a new song with the name specified in <see cref="NewSongName"/>
    /// and adds it to the <see cref="StoredSongs"/> collection.
    /// Besides, it also sets the <see cref="MainWindowViewModel.Song"/> to the newly created song.
    /// </summary>
    public void NewSong()
    {
        Logger.Task($"Creating new song \"{NewSongName}\"... ");
        var song = new SongDto
        {
            Name = NewSongName,
        };

        song.Preferences = new PreferencesDto
        {
            Volume = 50.0,
            Length = 4,
            Bpm = 256,
            Song = song,
        };

        StoredSongs.Add(song);
        MainVm.Song = song;
        NewSongName = string.Empty;
        
        this.RaisePropertyChanged(nameof(MainVm.Song));
        this.RaisePropertyChanged(nameof(StoredSongs));
        this.RaisePropertyChanged(nameof(NewSongName));
        Logger.Complete("Song created.");
    }
}