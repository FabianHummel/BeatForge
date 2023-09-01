using System;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Styling;
using BeatForgeClient.Utility;
using BeatForgeClient.ViewModels;

namespace BeatForgeClient.Views;

public partial class Content : UserControl
{
    public Content()
    {
        InitializeComponent();
    }
    
    private static readonly Animation _highlightAnimation = new()
    {
        Duration = TimeSpan.FromMilliseconds(200),
        PlaybackDirection = PlaybackDirection.Normal,
        Easing = new LinearEasing(),
        IterationCount = new IterationCount(1),
        FillMode = FillMode.Forward,
        Children =
        {
            new KeyFrame
            {
                Cue = new Cue(0.0),
                Setters = 
                {
                    new Setter(BackgroundProperty, Brushes.Black)
                }
            },
            
            new KeyFrame
            {
                Cue = new Cue(1.0),
                Setters = 
                {
                    new Setter(BackgroundProperty, Brushes.Transparent)
                }
            }
        }
    };
    
    private ContentViewModel ViewModel => (DataContext as ContentViewModel)!;

    // ReSharper disable once SuggestBaseTypeForParameter
    private void Grid_OnPointerPressed(object? _, PointerPressedEventArgs e)
    {
        var point = e.GetPosition(Grid);
        var pitch = (int) Math.Floor(point.Y / 10.0);
        var start = (int) Math.Floor(point.X / 20.0);
        ViewModel.ToggleNoteAt(start, pitch);
    }

    private void HighlightNotesAtHead()
    {
        foreach (var child in Notes.Children)
        {
            var animatable = child as Animatable;
            _highlightAnimation.RunAsync(animatable, null);
        }
    }

    private void Content_OnInitialized(object? sender, VisualTreeAttachmentEventArgs e)
    {
        // ViewModel.PropertyChanged += (_, args) =>
        // {
        //     if (args.PropertyName == nameof(ViewModel.Playback))
        //     {
        //         HighlightNotesAtHead();
        //     }
        // };
    }
}