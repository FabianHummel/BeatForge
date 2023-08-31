using System.ComponentModel.DataAnnotations.Schema;

namespace BeatForgeClient.Models;

[Table("n_note")]
public class Note
{
    public int Id { get; set; }
    public int Start { get; set; }
    public int Duration { get; set; }
    public int End => this.Start + this.Duration;
    public int Pitch { get; set; }
    public virtual Channel Channel { get; set; }
}

public class NoteDto
{
    public int? Id { get; set; }
    public int Start { get; set; }
    public int Duration { get; set; }
    public int End => this.Start + this.Duration;
    public int Pitch { get; set; }
    public ChannelDto Channel { get; set; } = null!;
}