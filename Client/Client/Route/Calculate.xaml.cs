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
using Client.Config;
namespace Client.Route
{
    /// <summary>
    /// Calculate.xaml 的交互逻辑
    /// </summary>
    public partial class Calculate : Page
    {
        public Calculate()
        {
            InitializeComponent();
            label2.Content = Ticket.From;
            label3.Content = Ticket.To;
            int dis = Data.getDistance(Ticket.From, Ticket.To);
            int onePrice = Data.getPrice(dis);
            label6.Content = onePrice.ToString();
            int allPrice = onePrice * Ticket.TicketNum;
            label1_Copy2.Content = Ticket.TicketNum.ToString();
            label6_Copy.Content = allPrice.ToString();
        }
        public Calculate(int onePrice)
        {
            InitializeComponent();
            label6.Content = onePrice.ToString();
            int allPrice = Ticket.TicketNum * onePrice;
            label1_Copy2.Content = Ticket.TicketNum.ToString();
            label6_Copy.Content = allPrice.ToString();
        }
    }
}
