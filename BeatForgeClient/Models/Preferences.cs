using System.ComponentModel.DataAnnotations.Schema;

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

public class PreferencesDto
{
    public int? Id { get; set; }
    public double Volume { get; set; }
    public int Length { get; set; }
    public int Bpm { get; set; }
    public SongDto? Song { get; set; }
}