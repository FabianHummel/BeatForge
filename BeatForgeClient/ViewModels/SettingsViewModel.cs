using System;
using System.Linq;
using BeatForgeClient.Infrastructure;
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

    public void SaveSong()
    {
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
        
        MainVm.TitlebarViewModel.LoadStoredSongs();
        MainVm.TitlebarViewModel.SelectedSong = MainVm.TitlebarViewModel
            .StoredSongs.First(s => s.Id == MainVm.Song.Id);
    }
}