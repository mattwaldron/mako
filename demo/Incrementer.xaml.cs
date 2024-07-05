using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;

namespace ADC.EngTools.UI;

public class ValueChangedEventArgs : EventArgs
{
    public int? Value { get; set; }
    public ValueChangedEventArgs(int? v)
    {
        Value = v;
    }
}
public partial class Incrementer : UserControl
{
    public int? Value { get; private set; }
    public event EventHandler<ValueChangedEventArgs> ValueChanged;
    private bool _useHex = false;
    public Incrementer(EventHandler<ValueChangedEventArgs> onChange = null)
    {
        Value = 0;
        ValueChanged = onChange;
        InitializeComponent();
    }

    // TODO: resize

    private void DecrementButton_Click(object sender, RoutedEventArgs e)
    {
        var next = Value - 1;
        ValueBox.Text = _useHex ? $"0x{next:x}" : $"{next}";
    }

    private void IncrementButton_Click(object sender, RoutedEventArgs e)
    {
        var next = Value + 1;
        ValueBox.Text = _useHex ? $"0x{next:x}" : $"{next}";
    }

    private void ValueBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        _useHex = (ValueBox.Text.Length > 2) && (ValueBox.Text.Trim().Substring(0, 2) == "0x" || ValueBox.Text.Trim().Substring(0, 2) == "0X");
        if (_useHex)
        {
            if (int.TryParse(ValueBox.Text.Substring(2), NumberStyles.HexNumber, null, out var newValue))
            {
                UpdateValue(newValue);
            }
            else
            {
                UpdateValue(null);
            }
        }
        else
        {
            if (int.TryParse(ValueBox.Text, NumberStyles.Integer, null, out var newValue))
            {
                UpdateValue(newValue);
            }
            else
            {
                UpdateValue(null);
            }
        }
    }

    private void UpdateValue(int? value)
    {
        Value = value;
        if (value is null)
        {
            ValueBox.Background = Brushes.Red;
        }
        else
        {
            ValueBox.Background = Brushes.Transparent;
            
        }
        ValueChanged?.Invoke(this, new ValueChangedEventArgs(Value));
    }
}
