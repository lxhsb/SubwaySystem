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
            public int Money;
            public override string ToString()
            {
                return Cardid;
            }
        }
        public Index()
        {
            InitializeComponent();
            refresh();
         //   listBox.SelectedIndex = 0;
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

        private void button_Click(object sender, RoutedEventArgs e)//add money 
        {
            object now = listBox.SelectedItem;
            if (now == null)
                return;
            User usernow = (User)now ;
            string text = textBox.Text;
            int money;
            if(!int.TryParse(text,out money) )
            {
                MessageBox.Show("fail");
            }
            else
            {
                string res = Ipc.Client.AddMoney(usernow.Cardid,money);
                if (res!="-1")
                {
                    MessageBox.Show("Add Money Success");
                }
                else
                {
                    MessageBox.Show("fail");
                }
            }
            refresh();
        }
        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object now = listBox.SelectedItem;
            if (now == null)
                return;
            User user = (User)(now);
            label2.Content = user.Money;
            label3.Content = user.Peopleid;
        }

        private void button1_Click(object sender, RoutedEventArgs e)//删除 
        {
            object obj = listBox.SelectedItem;
            if (obj == null)
                return;
            User user = (User)(obj);
            if(user.Money>0)
            {
                MessageBox.Show("无法删除余额大于0的用户,请尝试刷新");
            }
            else
            {
                MessageBox.Show(Ipc.Client.DelUser(user.Cardid));
            }
        }
    }
}
