using System.ComponentModel.DataAnnotations.Schema;

namespace BeatForgeClient.Infrastructure;

[Table("p_preferences")]
public class Preferences
{
    public int Id { get; set; }
    public double Volume { get; set; }
    public int SongId { get; set; }
    public Song Song { get; set; }
}

public class PreferencesDto
{
    public int? Id { get; set; }
    public double? Volume { get; set; }
    public int? SongId { get; set; }
    public SongDto? Song { get; set; }
}