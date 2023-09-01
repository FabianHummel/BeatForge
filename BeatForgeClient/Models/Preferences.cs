using System.ComponentModel.DataAnnotations.Schema;
using ReactiveUI;

namespace BeatForgeClient.Models;

[Table("p_preferences")]
public class Preferences
{
    public int Id { get; set; }
    public double Volume { get; set; }
    public int Length { get; set; }
    public int Bpm { get; set; }
    public int SongId { get; set; }
    public virtual Song Song { get; set; }
}

public class PreferencesDto : ReactiveObject
{
    private double _volume;
    private int _length;
    private int _bpm;
    private SongDto? _song;
    
    public int? Id { get; set; }

    public double Volume
    {
        get => _volume;
        set => this.RaiseAndSetIfChanged(ref _volume, value);
    }

    public int Length
    {
        get => _length;
        set => this.RaiseAndSetIfChanged(ref _length, value);
    }

    public int Bpm
    {
        get => _bpm;
        set => this.RaiseAndSetIfChanged(ref _bpm, value);
    }

    public SongDto? Song
    {
        get => _song;
        set => this.RaiseAndSetIfChanged(ref _song, value);
    }
}