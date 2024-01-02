using Mako.Ui;
using Microsoft.Maui.Controls;

namespace Mako.Testbench
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void SValueSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            SRadialMeter.Value = SValueSlider.Value;
            SLinearMeter.Value = SValueSlider.Value;
        }

        private void STextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SRadialMeter == null) return;
            var tb = sender as Entry;
            if (double.TryParse(tb.Text, out var v))
            {
                switch (tb.StyleId)
                {
                    case "SMinBox": SRadialMeter.Min = v; SLinearMeter.Min = v; break;
                    case "SMaxBox": SRadialMeter.Max = v; SLinearMeter.Max = v; break;
                    case "SWarningBox": SRadialMeter.WarningFraction = v; SLinearMeter.WarningFraction = v; break;
                    case "SHazardBox": SRadialMeter.HazardFraction = v; SLinearMeter.HazardFraction = v; break;
                }
            }
        }

    }
}
