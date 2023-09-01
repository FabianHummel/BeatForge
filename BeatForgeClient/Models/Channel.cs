using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using Avalonia.Media;
using BeatForgeClient.ViewModels;
using ReactiveUI;

namespace BeatForgeClient.Models;

[Table("c_channel")]
public class Channel
{
    public int Id { get; private set; }
    public string Name { get; set; }
    public float Volume { get; set; }
    public virtual IEnumerable<Note> Notes { get; set; } = new List<Note>();
    public virtual Instrument Instrument { get; set; }
    public virtual Song Song { get; set; }
}

public class ChannelDto : ReactiveObject
{
    private float _volume;
    private string? _name;
    private Instrument _instrument = Instrument.Square;
    private SongDto _song = null!;
    private bool _muted;

    private static Random Random { get; } = new();
    
    public int? Id { get; set; }

    public string? Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    public float Volume
    {
        get => _volume;
        set => this.RaiseAndSetIfChanged(ref _volume, value);
    }

    public ObservableCollection<NoteDto> Notes { get; set; } = new();

    public Instrument Instrument
    {
        get => _instrument;
        set => this.RaiseAndSetIfChanged(ref _instrument, value);
    }

    public SongDto Song
    {
        get => _song;
        set => this.RaiseAndSetIfChanged(ref _song, value);
    }

    public bool Muted
    {
        get => _muted;
        set => this.RaiseAndSetIfChanged(ref _muted, value);
    }

    public IBrush Color { get; } = Brush.Parse(
        $"#FF{Random.Next(128)+32:X}" +
        $"{Random.Next(128)+32:X}" +
        $"{Random.Next(128)+32:X}");

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
