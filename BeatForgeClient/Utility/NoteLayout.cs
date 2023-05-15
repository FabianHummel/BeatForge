using System;
using Avalonia;
using Avalonia.Layout;
using BeatForgeClient.Infrastructure;
using BeatForgeClient.Models;

namespace BeatForgeClient.Utility;

public class NoteLayout : VirtualizingLayout
{
    public static readonly StyledProperty<int> BeatCountProperty =
        AvaloniaProperty.Register<NoteLayout, int>(nameof(BeatCount));
        
    public static readonly StyledProperty<int> OctaveCountProperty =
        AvaloniaProperty.Register<NoteLayout, int>(nameof(OctaveCount));
        
    public static readonly StyledProperty<double> NoteWidthProperty =
        AvaloniaProperty.Register<NoteLayout, double>(nameof(NoteWidth));
        
    public static readonly StyledProperty<double> NoteHeightProperty =
        AvaloniaProperty.Register<NoteLayout, double>(nameof(NoteHeight));
        
    public int BeatCount
    {
        get => GetValue(BeatCountProperty);
        set => SetValue(BeatCountProperty, value);
    }
    
    public int OctaveCount
    {
        get => GetValue(OctaveCountProperty);
        set => SetValue(OctaveCountProperty, value);
    }
    
    public double NoteWidth
    {
        get => GetValue(NoteWidthProperty);
        set => SetValue(NoteWidthProperty, value);
    }
    
    public double NoteHeight
    {
        get => GetValue(NoteHeightProperty);
        set => SetValue(NoteHeightProperty, value);
    }

    protected override Size MeasureOverride(VirtualizingLayoutContext context, Size availableSize)
    {
        for (var i = 0; i < context.ItemCount; i++)
        {
            if (context.GetItemAt(i) is NoteDto note)
            {
                // Only arrange notes that are visible
                // Recycle notes
                var child = context.GetOrCreateElementAt(i);
                child.Arrange(new(note.Start*NoteWidth, note.Pitch*NoteHeight, NoteWidth, NoteHeight));
            }
        }

        return new(
            BeatCount * NoteWidth * 4,
            OctaveCount * NoteHeight * 8);
    }

    protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
    {
        base.OnPropertyChanged(change);
        
        if (change.Property.Equals(BeatCountProperty) ||
            change.Property.Equals(OctaveCountProperty) ||
            change.Property.Equals(NoteWidthProperty) ||
            change.Property.Equals(NoteHeightProperty))
            InvalidateMeasure();
    }
}