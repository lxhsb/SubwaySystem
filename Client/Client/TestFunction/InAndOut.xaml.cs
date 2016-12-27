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
using Client.Config;
using Client.Route;
namespace Client.TestFunction
{
    /// <summary>
    /// InAndOut.xaml 的交互逻辑
    /// </summary>
    public partial class InAndOut : Window
    {
        public InAndOut()
        {
            InitializeComponent();
            string[][] all = Data.getAllStation();
            for(int i= 1;i<=2;i++)
            {
                for(int j= 0;j<all[i].Length; j++)
                {
                    comboBox.Items.Add(all[i][j]);
                }
            }
            comboBox.SelectedIndex = 0;
        }
       

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Route.Status.GetOut(textBox.Text, comboBox.SelectedItem as string));
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(textBox.Text + "   " + comboBox.SelectedItem.ToString());
            MessageBox.Show(Route.Status.GetIn(textBox.Text, comboBox.SelectedItem as string));
        }
    }
}
