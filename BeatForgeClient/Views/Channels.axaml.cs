using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace BeatForgeClient.Views;

public partial class Channels : UserControl
{
    public Channels()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}