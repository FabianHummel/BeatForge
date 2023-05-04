using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeatForgeClient.Infrastructure;

[Table("c_channel")]
public class Channel
{
    public int Id { get; private set; }
    public string Name { get; set; }
    public double Volume { get; set; }
    public virtual List<Note> Notes { get; set; }
    public virtual Instrument Instrument { get; set; }
    public virtual Song Song { get; set; }
}

public class ChannelDto
{
    public string? Name { get; set; }
    public double? Volume { get; set; }
    public List<NoteDto>? Notes { get; set; }
    public InstrumentDto? Instrument { get; set; }
    public SongDto? Song { get; set; }
}