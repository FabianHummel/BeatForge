using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using ReactiveUI;

namespace BeatForgeClient.Models;

[Table("s_song")]
public class Song
{
    public int Id { get; private set; } 
    public string Name { get; set; }
    public virtual IEnumerable<Channel> Channels { get; } = new List<Channel>();
    public virtual Preferences Preferences { get; set; }
}

public class SongDto : ReactiveObject
{
    private string? _name;
    private PreferencesDto _preferences = null!;

    public int? Id { get; set; }

    public string? Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    public ObservableCollection<ChannelDto> Channels { get; } = new();

    public PreferencesDto Preferences
    {
        get => _preferences;
        set => this.RaiseAndSetIfChanged(ref _preferences, value);
    }
}