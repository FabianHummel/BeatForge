using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace BeatForgeClient.Views;

public partial class Content : UserControl
{
    public Content()
    {
        InitializeComponent();
        InitializeGrid();
        // PopulateGrid();
    }

    private void InitializeGrid()
    {
        // 3 Octaves at 12 notes per octave
        for (var i = 0; i < 12*3; i++)
        {
            Grid.RowDefinitions.Add(new RowDefinition(10.0, GridUnitType.Pixel));
        }
        
        // 4 beats at 16 notes per beat
        for (var i = 0; i < 4*16; i++)
        {
            Grid.ColumnDefinitions.Add(new ColumnDefinition(20.0, GridUnitType.Pixel));
        }
    }

    private void PopulateGrid()
    {
        var rnd = new System.Random();
        for (var i = 0; i < 200; i++)
        {
            var panel = new Panel();
            panel.Background = Brushes.Aquamarine;
            panel.SetValue(Grid.RowProperty, rnd.Next(0, 12*3));
            panel.SetValue(Grid.ColumnProperty, rnd.Next(0, 4*16));
            Grid.Children.Add(panel);
        }
    }
}