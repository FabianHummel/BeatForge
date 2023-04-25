using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeatForgeClient.Infrastructure;

[Table("c_channel")]
public class Channel
{
    public int Id { get; private set; }
    public string Name { get; set; }
    public List<Note> Notes { get; set; } = new();
    public Instrument Instrument { get; set; }
    public Song Song { get; set; }
}