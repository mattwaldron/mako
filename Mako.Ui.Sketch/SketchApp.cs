using System;
using System.Windows;

namespace Mako.Ui.Sketch;

public class SketchApp : Application
{
    private SketchWindow _window;
    private SketchWindow.Setup _guiSetup;

    public SketchApp(SketchWindow.Setup setup)
    {
        _guiSetup = setup;
    }

    protected override void OnStartup(StartupEventArgs args)
    {
        base.OnStartup(args);

        _window = new SketchWindow();
        _window.Loaded += (s, e) =>
        {
            _guiSetup(_window);
        };

        _window.ContentRendered += (s, e) =>
        {
            _window.Height = Math.Ceiling(_window.ControlPanel.ActualHeight) + 50; // 50 seems about right for the title bar... TODO: calculate for real
        };

        _window.Show();
    }
}
