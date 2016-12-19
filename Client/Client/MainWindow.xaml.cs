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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Client.Route;
using Client.Admin;
using Client.TestFunction;
using Client.Config;
using Client.Ipc;
namespace Client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Data.InitData("紫荆山");//设置默认为紫荆山
            string[][] RouteList = Data.getAllStation();
            for(int i=1;i<=2;i++)
            {
                for(int j = 0;j<RouteList[i].Length;j++)
                {
                    comboBox.Items.Add(RouteList[i][j]);
                }
            }
            comboBox.SelectedIndex = 10;
            Ticket.TicketNum = 1;
            try
            {
                Ipc.Client.Connect();
            }
            catch (Exception e )
            {
                MessageBox.Show(e.ToString());
                MessageBox.Show("连接失败");
                System.Environment.Exit(0);
            }
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            Button now = sender as Button;
            label.Content = now.Content;
            string name = now.Content.ToString();
            string src="";
            switch (name)
            {
                case "总线":
                    {
                        src = "AllRoute.xaml";
                        break;
                    }
                case "1号线":
                    {
                        src = "Route1.xaml";
                        break;
                    }
                case "2号线":
                    {
                        src = "Route2.xaml";
                        break;
                    }
                default:
                    {
                        src = "AllRoute.xaml";
                        break;
                    }
            }
            string path = "/Route/"+src;
            Uri source = new Uri(path, UriKind.Relative);
            object obj = null;
            try
            {
                obj = Application.LoadComponent(source);
            }
           catch
            {
                MessageBox.Show("fuck");
                return;
            }
            Page p = obj as Page;
            
            if(p!=null)
            {
                frame.NavigationService.RemoveBackEntry();          
                frame.Source = source;
                return;
            }
              
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
          //  this.Hide();
            Login login = new Login();
            login.ShowDialog();
        }
        public  int getTicketNum()
        {
            if (radioButton.IsChecked == true)
                return 1;
            else if (radioButton_Copy.IsChecked == true)
                return 2;
            else if (radioButton_Copy1.IsChecked == true)
                return 3;
            else if (radioButton_Copy2.IsChecked == true)
                return 4;
            else if (radioButton_Copy3.IsChecked == true)
                return 5;
            else if (radioButton_Copy4.IsChecked == true)
                return 6;
            else if (radioButton_Copy5.IsChecked == true)
                return 7;
            else if (radioButton_Copy6.IsChecked == true)
                return 8;
            else if (radioButton_Copy7.IsChecked == true)
                return 9;
            else
                return 1;
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            Button now = sender as Button;
            int ticketNum = getTicketNum();
            string strPrice = now.Content.ToString()[0].ToString();
            int price = int.Parse(strPrice);
            Pay pay = new Pay(price);
            pay.Show();
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            InAndOut i = new InAndOut();
            i.Show();
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string now = (string)comboBox.SelectedItem;
            Data.ResetDefaultStation(now);
        }

        private void radioButton_Copy7_Checked(object sender, RoutedEventArgs e)
        {
            Ticket.TicketNum = getTicketNum();
        }
    }
}
