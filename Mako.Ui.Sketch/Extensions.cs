using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Mako.Ui.Sketch;

public static class Extensions
{
    public static Panel AddUiElement(this Panel panel, UIElement elem)
    {
        if (panel is Grid grid)
        {
            var nextColumn = grid.ColumnDefinitions.Count;
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            Grid.SetColumn(elem, nextColumn);
        }
        panel.Children.Add(elem);
        return panel;
    }

    public static Panel AddButton(this Panel panel, string label, Color bcolor, Action action)
    {
        var button = new Button();
        button.Content = label;
        button.Click += (s, e) => action();
        button.Margin = new Thickness(4);
        var borderBrush = new SolidColorBrush(bcolor);
        button.Background = borderBrush;
        return panel.AddUiElement(button);
    }

    public static Panel AddButton(this Panel panel, string label, Action action)
    {
        return panel.AddButton(label, SystemColors.ControlColor, action);
    }

    public static Panel AddCheckBox(this Panel panel, string label, Action<bool> onChange)
    {
        var checkbox = new CheckBox();
        checkbox.Content = label;
        checkbox.Checked += (s, e) => onChange(true);
        checkbox.Unchecked += (s, e) => onChange(false);

        checkbox.Margin = new Thickness(4);
        return panel.AddUiElement(checkbox);
    }
}
