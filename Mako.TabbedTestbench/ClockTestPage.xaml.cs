using Mako.Ui;

namespace Mako.TabbedTestbench;

public partial class ClockTestPage : ContentPage
{
    private TimeOnly time;
    private Timer incrementTime = null;
    public ClockTestPage()
    {
        InitializeComponent();

        DigitalClock.Text = time.ToShortTimeString();

        incrementTime = new Timer((t) =>
        {
            time = time.AddMinutes(1);
            Clock.Hour = time.Hour;
            Clock.Minute = time.Minute;
            DigitalClock.Dispatcher.Dispatch(() => DigitalClock.Text = time.ToShortTimeString());
        }, null, Timeout.Infinite, 500);
        Clock.CanChange = true;
        Clock.PropertyChanged += (s, e) =>
        {
            var c = (AnalogClock)s;
            time = new TimeOnly(c.Hour, c.Minute);
            DigitalClock.Text = time.ToShortTimeString();
        };
    }

    private void EditMode_Toggled(object sender, ToggledEventArgs e)
    {
        if (e.Value == false)
        {
            incrementTime.Change(Timeout.Infinite, 500);
            Clock.CanChange = true;
        }
        else
        {
            Clock.CanChange = false;
            incrementTime.Change(500, 500);
        }
    }
}
