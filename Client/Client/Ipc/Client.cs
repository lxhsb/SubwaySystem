using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
namespace Client.Ipc
{
    /*
    整个系统只有
    Login 登录
    AddAccount 新建账户
    DelAcoount 删除账户
    AddMoney 加钱
    QueryAllAcoount 查询所有的非临时票用户
    QueryTempTicket 付款成功后向服务器发送请求，服务器返回临时卡卡号
    */
    struct Request
    {
        string Method;
        string Params;
    }
    struct Response
    {
        string Code;
        string Body;
    }
    class Client
    {
        private static Socket ClientSocket;
        private static string Ip = "127.0.0.1";//server ip 
        private static int Port = 1208;//server port 
        private static IPAddress ServerIp = IPAddress.Parse(Ip);
        private static byte[] Result = new byte[1 << 20];//缓冲区开 1M  不够再开

        private Client() { }//禁止实例化
        public static void Connect() //会抛出异常
        {
            ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                ClientSocket.Connect(new IPEndPoint(ServerIp, Port));
            }
            catch (Exception e)
            {
                throw e;//连接失败会抛异常
            }
        }
        



        

    }
}
