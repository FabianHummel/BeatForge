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
    public int End => this.Start + this.Duration;
    public int Pitch { get; set; }
    public virtual Channel Channel { get; set; }
}

public class NoteDto : ViewModelBase
{
    private int _pitch;
    public int? Id { get; set; }
    public int Start { get; set; }
    public int Duration { get; set; }
    public int End => this.Start + this.Duration;

    public int Pitch
    {
        get => _pitch;
        set => this.RaiseAndSetIfChanged(ref _pitch, value);
    }

    public ChannelDto Channel { get; set; } = null!;
}