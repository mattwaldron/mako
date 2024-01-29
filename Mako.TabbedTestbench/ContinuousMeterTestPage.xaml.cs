using Mako.Ui;
using Microsoft.Maui.Controls;

namespace Mako.TabbedTestbench;

public partial class ContinuousMeterTestPage : ContentPage
{
    public ContinuousMeterTestPage()
    {
        InitializeComponent();
    }

    private void ValueSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        RadialMeter.Value = ValueSlider.Value;
        LinearMeter.Value = ValueSlider.Value;
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (RadialMeter == null) return;
        var tb = sender as Entry;
        if (double.TryParse(tb.Text, out var v))
        {
            switch (tb.StyleId)
            {
                case "MinBox": RadialMeter.Min = v; LinearMeter.Min = v; break;
                case "MaxBox": RadialMeter.Max = v; LinearMeter.Max = v; break;
                case "WarningBox": RadialMeter.WarningFraction = v; LinearMeter.WarningFraction = v; break;
                case "HazardBox": RadialMeter.HazardFraction = v; LinearMeter.HazardFraction = v; break;
            }
        }
    }

}
