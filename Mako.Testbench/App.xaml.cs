using Microsoft.Maui.Controls;

namespace Mako.Testbench
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
