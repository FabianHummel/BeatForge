using System;
using System.Linq;
using BeatForgeClient.Extensions;
using BeatForgeClient.Infrastructure;
using BeatForgeClient.Models;

namespace BeatForgeClient.ViewModels;

public class SettingsViewModel : ViewModelBase
{
    public MainWindowViewModel MainVm { get; }
    public BeatForgeContext Db => MainVm.Db;
    
    public SettingsViewModel(MainWindowViewModel mainVm)
    {
        MainVm = mainVm;
    }

    public void TogglePlaySong()
    {
        MainVm.ContentViewModel.Playing = !MainVm.ContentViewModel.Playing;
    }

    public void SaveSong()
    {
        Logger.Task("Saving song... ");
        
        if (MainVm.TitlebarViewModel.SelectedSong is null) return;
        var songDb = Db.Songs.FirstOrDefault(s => 
            s.Id == MainVm.TitlebarViewModel.SelectedSong.Id);
        if (songDb is null)
        {
            var song = Program.Mapper.Map<Song>(MainVm.TitlebarViewModel.SelectedSong);
            Db.Songs.Add(song);
            Db.SaveChanges();
            MainVm.TitlebarViewModel.SelectedSong.Id = song.Id;
        }
        else
        {
            var song = Program.Mapper.Map(MainVm.TitlebarViewModel.SelectedSong, songDb);
            Db.Songs.Update(song);
            Db.SaveChanges();
        }
        
        Logger.Complete("Song saved.");
        Logger.Task("Refreshing Songs... ");
        MainVm.TitlebarViewModel.LoadStoredSongs();
        Logger.Complete("Songs refreshed.");
    }
}