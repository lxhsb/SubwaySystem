using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Web.Script.Serialization;
using Client.Tools;
namespace Client.Ipc
{
    //为了日后的可扩展性  即使现在用不到超出ascii码的部分  我们依然使用utf8 
    //go服务端那部分默认使用的也是utf8
    //ToDo 不知道如何判断超时 等网络传输时候存在的问题
    /*
    整个系统只有
    Login 登录
    REG 新建账户
    Del 删除账户
    AddMoney 加钱
    QueryAllAcoount 查询所有的非临时票用户
    QueryTempTicket 付款成功后向服务器发送请求，服务器返回临时卡卡号
    */
    class Client
    {
        public struct Request
        {
            public string Method;
            public string Params;
            public Request(string _method ,string _params)
            {
                Method = _method;
                Params = _params;
            }

        };
        public struct Response
        {
            public string Code;
            public string Body;
            public Response(string _code ,string _body)
            {
                Code = _code;
                Body = _body;
            }
        };
         public struct Account //这个是登陆用的
        {
           public  string user;
           public  string pass;
           public Account(string u ,string p)
            {
                user = u;
                pass = p;
            }
        }
        
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
        public static Response Send(Request req)
        {
            Response rep;
            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                string str = js.Serialize(req);
                
                ClientSocket.Send(Encoding.UTF8.GetBytes(str));
                int len = ClientSocket.Receive(Result);
                string ans = Encoding.UTF8.GetString(Result, 0, len);
                 rep = js.Deserialize<Response>(ans);
            }
            catch (Exception e )
            {
                throw e;
            }
            return rep;
        }
        public static bool Login(string user ,string pass)
        {
            try
            {
                pass = Tools.Tools.getMd5(pass);
                Account a = new Account(user, pass);
                JavaScriptSerializer js = new JavaScriptSerializer();
                string a_json = js.Serialize(a);
                Request req = new Request("LOGIN", a_json);
                Response rep = Send(req);
                return rep.Body == "YES";
            }
            catch (Exception e )
            {
                throw e;
            } 
        }
        public static string Reg(string peopleid)
        {
            try
            {
                Request req = new Request();
                req.Method = "REG";
                req.Params = peopleid;
                JavaScriptSerializer js = new JavaScriptSerializer();
                string a_json = js.Serialize(req);
                Response rep = Send(req);
                return rep.Body;
            }
            catch(Exception e )
            {
                throw e;
            }
        }
        public static string GetAllFrequentUserList()//返回一个json
        {
            Request req = new Request();
            req.Method = "GETALLFREQUENTUSERLIST";
            req.Params = "";
            JavaScriptSerializer js = new JavaScriptSerializer();
          
            
            Response re = Send(req);
            return re.Body;
            



        }
    }

}
