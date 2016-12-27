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
        private Calculate cal;
        public Pay()
        {
            InitializeComponent();
            cal = new Calculate();
            frame.NavigationService.Navigate(cal);
        }
        public Pay(int price)
        {
            InitializeComponent();
            cal = new Calculate(price);
            frame.NavigationService.Navigate(cal);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            cal.checkThread.Abort();//关闭监视线程  实在想不到更好的解决办法了 （wszz）
        }
    }
}
