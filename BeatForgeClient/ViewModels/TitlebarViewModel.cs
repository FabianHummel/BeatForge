using System.Collections.ObjectModel;
using System.Linq;
using BeatForgeClient.Extensions;
using BeatForgeClient.Infrastructure;
using BeatForgeClient.Models;
using ReactiveUI;

namespace BeatForgeClient.ViewModels;

public class TitlebarViewModel : ViewModelBase
{
    public MainWindowViewModel MainVm { get; }
    public BeatForgeContext Db => MainVm.Db;
    
    public TitlebarViewModel(MainWindowViewModel mainVm)
    {
        MainVm = mainVm;
        LoadStoredSongs();
    }

    public string Title { get; set; } = "BeatForge";
    public string NewSongName { get; set; } = string.Empty;
    
    public ObservableCollection<SongDto> StoredSongs { get; } = new();
    
    private SongDto? _selectedSong;

    public SongDto? SelectedSong
    {
        get => _selectedSong;
        set
        {
            var data = LoadFullSong(value!);
            this.RaiseAndSetIfChanged(ref _selectedSong, data);
        }
    }

    /// <summary>
    /// Loads all songs from the database and into the
    /// <see cref="StoredSongs"/> collection.
    /// Note that StoredSongs only holds values that are present in the
    /// "Songs" table, and does not include associated channels or preferences.
    /// See <see cref="LoadFullSong"/> for loading the full data.
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
    public SongDto LoadFullSong(SongDto selectedSong)
    {
        Logger.Task($"Loading song \"{selectedSong.Name}\"... ");
        
        var song = Db.Songs.FirstOrDefault(s => s.Id == selectedSong.Id);
        var songDto = song is null 
            ? StoredSongs.First(s => s.Id == selectedSong.Id) 
            : Program.Mapper.Map<SongDto>(song);
        
        Logger.Complete("Song loaded.");
        return songDto;
    }
    
    /// <summary>
    /// Creates a new song with the name specified in <see cref="NewSongName"/>
    /// and adds it to the <see cref="StoredSongs"/> collection.
    /// Besides, it also sets the <see cref="TitlebarViewModel.SelectedSong"/> to the newly created song.
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
        MainVm.TitlebarViewModel.SelectedSong = song;
        NewSongName = string.Empty;
        
        this.RaisePropertyChanged(nameof(StoredSongs));
        this.RaisePropertyChanged(nameof(NewSongName));
        Logger.Complete("Song created.");
    }
}