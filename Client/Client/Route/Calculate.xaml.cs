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
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Client.Config;
namespace Client.Route
{
    /// <summary>
    /// Calculate.xaml 的交互逻辑
    /// </summary>
    public partial class Calculate : Page
    {
        private bool isPaid;//whether it is paid
//        private Thread now;//主窗体线程
        private void check()//to do
        {

            while(isPaid ==false){}
            if (isPaid)
            {
                MessageBox.Show("paid success");
                string[] ans = Ipc.Client.AskTempUser(num, onePrice);
                if (ans == null)
                {
                    MessageBox.Show("error");
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("购票成功");
                    for (int i = 0; i < ans.Length; i++)
                    {
                        sb.AppendLine(ans[i]);
                    }
                    MessageBox.Show(sb.ToString());
                }
            }
        }
        private int dis;
        private int onePrice;
        private int allPrice;
        private int num = Ticket.TicketNum;
        public Thread checkThread;//新建一个线程一直查询是否已经支付 （并不想设置成public）
        public Calculate()
        {
            InitializeComponent();
            label2.Content = Ticket.From;
            label3.Content = Ticket.To;
            dis = Data.getDistance(Ticket.From, Ticket.To);
            onePrice = Data.getPrice(dis);
            label6.Content = onePrice.ToString();
            allPrice = onePrice * Ticket.TicketNum;
            label1_Copy2.Content = Ticket.TicketNum.ToString();
            label6_Copy.Content = allPrice.ToString();
            checkThread = new Thread(new ThreadStart(this.check));
            checkThread.Start();
            

        }
        public Calculate(int onePrice)
        {
            InitializeComponent();
          //  now = Thread.CurrentThread;
            label6.Content = onePrice.ToString();
            allPrice = Ticket.TicketNum * onePrice;
            label1_Copy2.Content = Ticket.TicketNum.ToString();
            label6_Copy.Content = allPrice.ToString();
            checkThread = new Thread(new ThreadStart(this.check));
            checkThread.Start();
        }
        private void pay()//支付 
        {

            if (isPaid)
            {
                MessageBox.Show("已经支付成功，如果想继续支付请重新购票");

            }else
            {
                isPaid = true;
            }
            
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            label1_Copy5.Content = label6_Copy.Content;
            pay();
        }
    }
}
