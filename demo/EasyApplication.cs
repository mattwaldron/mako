using System;
using System.Windows;

namespace ADC.EngTools.UI
{
    public class EasyApplication : Application
    {
        private EasyWindow _window;
        private EasyWindow.Setup _guiSetup;

        public EasyApplication(EasyWindow.Setup setup)
        {
            _guiSetup = setup;
        }

        protected override void OnStartup(StartupEventArgs args)
        {
            base.OnStartup(args);

            _window = new EasyWindow();
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
}
