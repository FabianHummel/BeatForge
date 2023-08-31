using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeatForgeClient.Models;

[Table("c_channel")]
public class Channel
{
    public int Id { get; private set; }
    public string Name { get; set; }
    public float Volume { get; set; }
    public virtual List<Note> Notes { get; set; }
    public virtual Instrument Instrument { get; set; }
    public virtual Song Song { get; set; }
}

public class ChannelDto
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public float Volume { get; set; }
    public List<NoteDto> Notes { get; set; } = new();
    public Instrument Instrument { get; set; } = Instrument.Square;
    public SongDto Song { get; set; } = null!;
    public bool Muted { get; set; }
    
    public float ProcessedVolume
    {
        get => Muted ? 0 : Volume;
        set
        {
            Volume = value;
            if (value > 0.0f)
            {
                Muted = false;
            }
        }
    }
}
