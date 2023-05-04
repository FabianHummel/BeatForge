using System;
using System.Collections.ObjectModel;
using System.Linq;
using AutoMapper.QueryableExtensions;
using BeatForgeClient.Extensions;
using BeatForgeClient.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;

namespace BeatForgeClient.ViewModels;

public class TitlebarViewModel : ViewModelBase
{
    public MainWindowViewModel MainWindowViewModel { get; }
    public BeatForgeContext Db => MainWindowViewModel.Db;
    
    public TitlebarViewModel(MainWindowViewModel mainWindowViewModel)
    {
        MainWindowViewModel = mainWindowViewModel;
        LoadStoredSongs();
    }

    public string Title { get; set; } = "BeatForge";
    
    public string NewSongName { get; set; } = string.Empty;
    
    public ObservableCollection<SongDto> StoredSongs { get; } = new();

    public void LoadStoredSongs()
    {
        Console.Write("Loding stored songs... ");
        var songs = from s in Db.Songs
            select s;
        var projection = songs.ProjectTo<SongDto>(
            Program.Mapper.ConfigurationProvider);
        StoredSongs.ReplaceAll(projection);
        Console.WriteLine($"done. {StoredSongs.Count} songs loaded.");
    }
    
    public void NewSong()
    {
        Console.Write("Creating new song... ");
        var song = new SongDto
        {
            Name = NewSongName,
        };

        song.Preferences = new PreferencesDto
        {
            Volume = 50.0,
            Song = song,
        };

        StoredSongs.Add(song);
        
        MainWindowViewModel.Song = song;
        NewSongName = string.Empty;
        
        this.RaisePropertyChanged(nameof(MainWindowViewModel.Song));
        this.RaisePropertyChanged(nameof(NewSongName));
        Console.WriteLine("done.");
    }

    public void SaveSong()
    {
        if (MainWindowViewModel.Song is null) return;
        Console.WriteLine($"Saving song {MainWindowViewModel.Song.Name}... ");

        try
        {
            var songDb = Db.Songs.FirstOrDefault(s =>
                s.Id == MainWindowViewModel.Song.Id);
            if (songDb is null)
            {
                Console.WriteLine("Song not found in database, creating new song... ");
                var song = Program.Mapper.Map<Song>(MainWindowViewModel.Song);
                Db.Songs.Add(song);
                Db.SaveChanges();
                Console.WriteLine($"Song created with id {song.Id}.");
                MainWindowViewModel.Song.Id = song.Id;
                MainWindowViewModel.Song.Preferences.Id = song.Preferences.Id;
                MainWindowViewModel.Song.Preferences = Program.Mapper.Map<PreferencesDto>(song.Preferences);
            }
            else
            {
                Console.Write("Song found in database, updating song... ");
                // Program.Mapper.Map(MainWindowViewModel.Song, songDb);
                songDb.Name = MainWindowViewModel.SettingsViewModel.SongTitle;
                Db.SaveChanges();
                Console.WriteLine("done.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}