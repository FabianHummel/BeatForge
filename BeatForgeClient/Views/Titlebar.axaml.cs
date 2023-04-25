using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace BeatForgeClient.Views;

public partial class Titlebar : UserControl
{
    public Titlebar()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}