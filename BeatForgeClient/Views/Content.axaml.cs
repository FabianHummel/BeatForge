using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace BeatForgeClient.Views;

public partial class Content : UserControl
{
    public Content()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}