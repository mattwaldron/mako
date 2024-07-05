using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace ADC.EngTools.UI
{
    /// <summary>
    /// Interaction logic for TriStateSwitch.xaml
    /// </summary>
    public partial class TriStateSwitch : UserControl
    {
        public bool? Closed { get; private set; } = null;
        public event EventHandler<bool>? ValueChanged;
        public object UserContext { get; set; }

        public TriStateSwitch()
        {
            InitializeComponent();

            SetDisplay(Closed);
        }

        public void SetDisplay(bool? value)
        {
            SwitchImage.Source = value.HasValue 
                ?   (value.Value 
                    ?   SwitchImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/ADC.EngTools.UI;component/Images/switch_closed.png"))
                    :   SwitchImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/ADC.EngTools.UI;component/Images/switch_open.png")))
                : SwitchImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/ADC.EngTools.UI;component/Images/switch_unknown.png"));

            Closed = value;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Closed != null)
            {
                Closed = !Closed;
            }
            else
            {
                Closed = true;
            }
            SetDisplay(Closed);
            ValueChanged?.Invoke(this, Closed.Value);
        }
    }
}
