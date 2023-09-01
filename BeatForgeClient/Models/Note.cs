using System.ComponentModel.DataAnnotations.Schema;
using Avalonia.Media;
using BeatForgeClient.ViewModels;
using ReactiveUI;

namespace BeatForgeClient.Models;

[Table("n_note")]
public class Note
{
    public int Id { get; set; }
    public int Start { get; set; }
    public int Duration { get; set; }
    public int Pitch { get; set; }
    public virtual Channel Channel { get; set; }
}

public class NoteDto : ReactiveObject
{
    private int _pitch;
    private int _start;
    private int _duration;
    private ChannelDto _channel = null!;

    public int? Id { get; set; }

    public int Start
    {
        get => _start;
        set => this.RaiseAndSetIfChanged(ref _start, value);
    }

    public int Duration
    {
        get => _duration;
        set => this.RaiseAndSetIfChanged(ref _duration, value);
    }
    
    public int Pitch
    {
        get => _pitch;
        set => this.RaiseAndSetIfChanged(ref _pitch, value);
    }

    public ChannelDto Channel
    {
        get => _channel;
        set => this.RaiseAndSetIfChanged(ref _channel, value);
    }

    public int End => Start + Duration;
}