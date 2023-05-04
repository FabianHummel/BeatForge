using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeatForgeClient.Infrastructure;

[Table("s_song")]
public class Song
{
    public int Id { get; private set; } 
    public string Name { get; set; }
    public List<Channel> Channels { get; } = new();
    public Preferences Preferences { get; set; }
}

public class SongDto
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public List<ChannelDto> Channels { get; set; } = new();
    public PreferencesDto Preferences { get; set; }
}