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
using Client.Ipc;
using System.Web.Script.Serialization;
namespace Client.Admin
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Index : Window
    {
        public struct User
        {
            public string Cardid;
            public string Peopleid;
            public string Money;
            public override string ToString()
            {
                return "        "+Cardid + "                           " + string.Format("{0:0000}", Money.ToString()) + "                                " + Peopleid;
            }
        }
        public Index()
        {
            InitializeComponent();
            refresh();
        }
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            Reg reg = new Reg();
            reg.Show();
        }
        private void refresh()
        {
            string all = Ipc.Client.GetAllFrequentUserList();
            List<User> ans = new JavaScriptSerializer().Deserialize<List<User>>(all);
            listBox.Items.Clear();
            if (ans == null)
            {
                return;
            }
            foreach (User user in ans)
            {
                listBox.Items.Add(user);
            }
            listBox.SelectedIndex = 0;

           
           
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            refresh();
        }
    }
}
