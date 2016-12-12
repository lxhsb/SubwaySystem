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
using System.Windows.Shapes;


namespace Client.Route
{
    /// <summary>
    /// Pay.xaml 的交互逻辑
    /// </summary>
    public partial class Pay : Window
    {
        public Pay()
        {
            InitializeComponent();
            frame.NavigationService.Navigate(new Calculate());
        }
        public Pay(int price )
        {
            InitializeComponent();
            frame.NavigationService.Navigate(new Calculate(price));
        }
    }
}
