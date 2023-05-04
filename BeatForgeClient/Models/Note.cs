using System.ComponentModel.DataAnnotations.Schema;

namespace BeatForgeClient.Infrastructure;

[Table("n_note")]
public class Note
{
    public int Id { get; set; }
    public decimal Start { get; set; }
    public decimal Duration { get; set; }
    public decimal End => Start + Duration;
    public int Pitch { get; set; }
    public Channel Channel { get; set; }
}

public class NoteDto
{
    public int? Id { get; set; }
    public decimal? Start { get; set; }
    public decimal? Duration { get; set; }
    public decimal? End { get; set; }
    public int? Pitch { get; set; }
    public ChannelDto? Channel { get; set; }
}