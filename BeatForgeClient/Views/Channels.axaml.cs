using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using BeatForgeClient.ViewModels;

namespace BeatForgeClient.Views;

public partial class Channels : UserControl
{
    public Channels()
    {
        InitializeComponent();
    }
    
    private ChannelsViewModel ChannelsViewModel => (DataContext as ChannelsViewModel)!;

    private void InputElement_OnKeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.S:
                ChannelsViewModel.ToggleSingleCurrent();
                break;
            
            case Key.M:
                ChannelsViewModel.ToggleMuteCurrent();
                break;
        }
    }
}