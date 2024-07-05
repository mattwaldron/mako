using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mako.Ui.Sketch;

public class Sketch
{
    public static void Create(SketchWindow.Setup setup)
    {
        Thread.CurrentThread.SetApartmentState(ApartmentState.Unknown);
        Thread.CurrentThread.SetApartmentState(ApartmentState.STA);
        var a = new SketchApp(setup);
        a.DispatcherUnhandledException += (s, e) => { e.Handled = true; };
        TaskScheduler.UnobservedTaskException += (s, e) => { };
        AppDomain.CurrentDomain.UnhandledException += (s, e) => { };
        a.Run();
    }
}
