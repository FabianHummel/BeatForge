using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeatForgeClient.Infrastructure;

[Table("s_song")]
public class Song
{
    public int Id { get; private set; } 
    public string Name { get; set; }
    public List<Channel> Channels { get; set; } = new();
    public int PreferencesId { get; set; }
    public Preferences Preferences { get; set; }
}