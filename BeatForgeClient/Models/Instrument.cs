using System.ComponentModel;

namespace BeatForgeClient.Models;

public enum Instrument
{
    [Description("Sine Wave")]
    Sine,
    [Description("Square Wave")]
    Square,
    [Description("Triangle Wave")]
    Triangle,
    [Description("Pulse Wave")]
    Pulse,
    [Description("Sawtooth Wave")]
    Sawtooth,
    [Description("Cringe Wave")]
    Cringe
}
