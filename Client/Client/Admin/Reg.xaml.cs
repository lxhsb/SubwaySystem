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

namespace Client.Admin
{
    /// <summary>
    /// Reg.xaml 的交互逻辑
    /// </summary>
    public partial class Reg : Window
    {
        public Reg()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string peopleid = textBox1.Text;
            if(peopleid.Length!=18)
            {
                MessageBox.Show("请合法输入");
                return;
            }
            else
            {
                string res = Ipc.Client.Reg(peopleid);
                MessageBox.Show(res);
                this.Close();
            }

        }
    }
}
