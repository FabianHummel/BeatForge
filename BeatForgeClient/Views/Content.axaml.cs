using System;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Threading;
using BeatForgeClient.Models;
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

    private void Content_OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        ViewModel.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(ContentViewModel.Playback))
            {
                foreach (var child in Notes.Children)
                {
                    Dispatcher.UIThread.Post(() =>
                    {
                        if (child is Border { DataContext: NoteDto data } note && data.Start == ViewModel.Playback)
                        {
                            _highlightAnimation.RunAsync(note, null);
                        }
                    });
                }
            }
        };
    }

    private void Handle(PointerEventArgs e)
    {
        var point = e.GetPosition(Grid);
        var pitch = (int) Math.Floor(point.Y / 10.0);
        var start = (int) Math.Floor(point.X / 20.0);
        
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            ViewModel.SetNoteAt(start, pitch);
        }
        
        else if (e.GetCurrentPoint(this).Properties.IsRightButtonPressed)
        {
            ViewModel.RemoveNoteAt(start, pitch);
        }
    }
    
    // ReSharper disable once SuggestBaseTypeForParameter
    private void Grid_OnPointerPressed(object? _, PointerPressedEventArgs e)
    {
        Handle(e);
    }

    private void Grid_OnPointerMoved(object? _, PointerEventArgs e)
    {
        Handle(e);
    }
}