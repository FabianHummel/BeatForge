using System;
using System.Linq;
using BeatForgeClient.Extensions;
using BeatForgeClient.Infrastructure;
using BeatForgeClient.Models;
using ReactiveUI;

namespace BeatForgeClient.ViewModels;

public class SettingsViewModel : ViewModelBase
{
    public MainWindowViewModel MainVm { get; }
    public BeatForgeContext Db => MainVm.Db;
    
    public SettingsViewModel(MainWindowViewModel mainVm)
    {
        MainVm = mainVm;
        
        MainVm.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(MainVm.Song))
            {
                this.RaisePropertyChanged(nameof(SongTitle));
                this.RaisePropertyChanged(nameof(SongLength));
                this.RaisePropertyChanged(nameof(SongBpm));
            }
        };
    }

    public string SongTitle
    {
        get => MainVm.Song?.Name ?? "No Song Selected";
        set
        {
            if (MainVm.Song is null) return;
            MainVm.Song.Name = value;
            this.RaisePropertyChanged(nameof(MainVm.TitlebarViewModel.StoredSongs));
        }
    }

    public int SongLength
    {
        get => MainVm.Song?.Preferences.Length ?? 0;
        set
        {
            if (MainVm.Song is null) return;
            MainVm.Song.Preferences.Length = value;
            this.RaisePropertyChanged(nameof(MainVm.SettingsViewModel.SongLength));
        }
    }
    
    public int SongBpm
    {
        get => MainVm.Song?.Preferences.Bpm ?? 0;
        set
        {
            if (MainVm.Song is null) return;
            MainVm.Song.Preferences.Bpm = value;
            this.RaisePropertyChanged(nameof(MainVm.SettingsViewModel.SongBpm));
        }
    }

    public void TogglePlaySong()
    {
        MainVm.ContentViewModel.Playing = !MainVm.ContentViewModel.Playing;
    }

    public void SaveSong()
    {
        Logger.Task("Saving song... ");
        MainVm.ContentViewModel.SaveChannelNotes();
        MainVm.ChannelsViewModel.SaveSongChannels();
        
        if (MainVm.Song is null) return;
        var songDb = Db.Songs.FirstOrDefault(s => 
            s.Id == MainVm.Song.Id);
        if (songDb is null)
        {
            var song = Program.Mapper.Map<Song>(MainVm.Song);
            Db.Songs.Add(song);
            Db.SaveChanges();
            MainVm.Song.Id = song.Id;
        }
        else
        {
            var song = Program.Mapper.Map(MainVm.Song, songDb);
            Db.Songs.Update(song);
            Db.SaveChanges();
        }
        
        Logger.Complete("Song saved.");
        Logger.Task("Refreshing Songs... ");
        MainVm.TitlebarViewModel.LoadStoredSongs();
        MainVm.TitlebarViewModel.SelectedSong = MainVm.TitlebarViewModel
            .StoredSongs.First(s => s.Id == MainVm.Song.Id);
        Logger.Complete("Songs refreshed.");
    }
}