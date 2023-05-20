using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeatForgeClient.Models;

[Table("s_song")]
public class Song
{
    public int Id { get; private set; } 
    public string Name { get; set; }
    public virtual List<Channel> Channels { get; } = new();
    public virtual Preferences Preferences { get; set; }
}

public class SongDto
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public List<ChannelDto> Channels { get; } = new();
    public PreferencesDto Preferences { get; set; } = null!;
    
    public double Volume => Preferences.Volume;
    public int Length => Preferences.Length;
    public int Bpm => Preferences.Bpm;
    public SongDto? Song => Preferences.Song;
}