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
namespace Client.Admin
{
    /// <summary>
    /// login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string usr = textBox.Text;
            string pass = textBox1.Text;
            if(usr.Length<=0||usr.Length>=16||pass.Length<=0||pass.Length>=16)
            {
                MessageBox.Show("请合法输入");
            }
            else
            {
                if(Ipc.Client.Login(usr,pass)==true)
                {
                    Index i = new Index();
                    i.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("登录失败");
                }
            }
        }
    }
}
