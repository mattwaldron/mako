using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace ADC.EngTools.UI
{
    /// <summary>
    /// Interaction logic for EasyWindow.xaml
    /// </summary>
    public partial class EasyWindow : Window
    {
        public delegate void Setup(EasyWindow window);

        public EasyWindow()
        {
            InitializeComponent();
        }

        public void SetTitle(string s)
        {
            Title = s;
        }

        public void Log(UIElement elem)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                LogPanel.Children.Add(elem);
                LogScroll.ScrollToEnd();
            }));
        }
        public void Log(string s, Color color)
        {
            var text = new TextBlock();
            text.Text = s;
            text.Margin = new Thickness(4);
            text.TextWrapping = TextWrapping.Wrap;
            text.Foreground = new SolidColorBrush(color);
            Log(text);
        }

        public void Log(string s)
        {
            Log(s, Colors.Black);
        }

        public void Log(BitmapImage bmp)
        {
            var image = new Image();
            image.Source = bmp;
            image.StretchDirection = StretchDirection.DownOnly;
            Log(image);
        }

        public void Log(object o)
        {
            if (o is string s)
            {
                Log(s);
            }
            else if (o is BitmapImage bmp)
            {
                Log(bmp);
            }
            // TODO: What else could anyone possibly want to display???
        }

        public void ClearLog(string filename = "")
        {
            if (!string.IsNullOrEmpty(filename))
            {
                using (var f = new StreamWriter(filename))
                {
                    foreach (var item in LogPanel.Children)
                    {
                        if (item is TextBlock tb)
                        {
                            f.WriteLine(tb.Text);
                        }
                    }
                }
            }
            LogPanel.Children.Clear();
        }
    }
}
