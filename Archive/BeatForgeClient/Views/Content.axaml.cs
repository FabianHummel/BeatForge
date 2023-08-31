using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using BeatForgeClient.Utility;
using BeatForgeClient.ViewModels;

namespace BeatForgeClient.Views;

public partial class Content : UserControl
{
    public Content()
    {
        InitializeComponent();
    }
    
    private ContentViewModel ViewModel => (DataContext as ContentViewModel)!;

    // ReSharper disable once SuggestBaseTypeForParameter
    private void Grid_OnPointerPressed(object? _, PointerPressedEventArgs e)
    {
        var point = e.GetPosition(Grid);
        var pitch = (int) Math.Floor(point.Y / 10.0);
        var start = (int) Math.Floor(point.X / 20.0);
        ViewModel.ToggleNoteAt(start, pitch);
    }
}

// public class Editor : Control
// {
//     public class EditorDrawOperation : ICustomDrawOperation
//     {
        
//     }
// }