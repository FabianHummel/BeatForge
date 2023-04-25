using System.ComponentModel.DataAnnotations.Schema;

namespace BeatForgeClient.Infrastructure;

[Table("i_instrument")]
public class Instrument
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ChannelId { get; set; }
    public Channel Channel { get; set; }
}