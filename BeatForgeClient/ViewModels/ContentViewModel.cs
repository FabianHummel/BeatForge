using System.Collections.ObjectModel;
using System.Linq;
using BeatForgeClient.Extensions;
using BeatForgeClient.Models;
using ReactiveUI;

namespace BeatForgeClient.ViewModels;

public class ContentViewModel : ViewModelBase
{
    public MainWindowViewModel MainVm { get; }
    
    public ContentViewModel(MainWindowViewModel mainVm)
    {
        MainVm = mainVm;
        
        MainVm.ChannelsViewModel.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(MainVm.ChannelsViewModel.SelectedChannel))
            {
                LoadChannelNotes();
            }
        };
    }
    
    public ObservableCollection<NoteDto> ChannelNotes { get; } = new();

    public void LoadChannelNotes()
    {
        if (MainVm.ChannelsViewModel.SelectedChannel == null) return;
        Logger.Task("Loading notes... ");
        ChannelNotes.ReplaceAll(MainVm.ChannelsViewModel.SelectedChannel.Notes);
        Logger.Complete($"({ChannelNotes.Count} notes loaded).");
    }
    
    public void ToggleNoteAt(int start, int pitch)
    {
        if (MainVm.ChannelsViewModel.SelectedChannel == null) return;
        Logger.Task($"Toggling note at {start} {pitch}... ");

        var note = ChannelNotes.FirstOrDefault(n =>
            n.Channel == MainVm.ChannelsViewModel.SelectedChannel &&
            n.Start == start && 
            n.Pitch == pitch);
        if (note is not null)
        {
            ChannelNotes.Remove(note);
        }
        else
        {
            ChannelNotes.Add(new()
            {
                Start = start,
                Pitch = pitch,
                Channel = MainVm.ChannelsViewModel.SelectedChannel,
                Duration = 1
            });
        }
        
        this.RaisePropertyChanged(nameof(ChannelNotes));
        var added = note is null ? "added" : "removed";
        Logger.Complete($"Note {added}.");
    }
    
    public void SaveChannelNotes()
    {
        if (MainVm.ChannelsViewModel.SelectedChannel == null) return;
        Logger.Task("Saving notes... ");
        MainVm.ChannelsViewModel.SelectedChannel.Notes.ReplaceAll(ChannelNotes);
        Logger.Complete("Notes saved.");
    }
}