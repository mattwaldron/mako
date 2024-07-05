using System;
using System.Threading;
using System.Threading.Tasks;

namespace ADC.EngTools.UI;

public class Ui
{
    public static void Create(EasyWindow.Setup setup)
    {
        Thread.CurrentThread.SetApartmentState(ApartmentState.Unknown);
        Thread.CurrentThread.SetApartmentState(ApartmentState.STA);
        var a = new EasyApplication(setup);
        a.DispatcherUnhandledException += (s, e) => { e.Handled = true; };
        TaskScheduler.UnobservedTaskException += (s, e) => { };
        AppDomain.CurrentDomain.UnhandledException += (s, e) => { };
        a.Run();
    }
}
