using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Config;
using Client.Ipc;
namespace Client.Route
{
    class Status
    {
        /*
        这个是进出站用的
        */
        private Status(){}
        private static Dictionary<string, string> mp = new Dictionary<string, string>();
       public static string GetIn(string id ,string loc)
        {
            if (id == "")
            {
                return "id 不合法";
            }
            if(mp.ContainsKey(id))
            {
                return "不能重复进站";
            }
            else
            {
                mp[id] = loc;
                return "进站成功";
            }
        }
        public static string GetOut(string id ,string loc )
        {
            if (id =="")
            {
                return "id 不合法";
            }
            if (!mp.ContainsKey(id))
            {
                return "还没进站 无法出战";
            }
            else
            {
                int dis = Data.getDistance(mp[id], loc);
                int price = Data.getPrice(dis);
                string ans = Ipc.Client.GetOut(id, price);
                return ans;

            }
        }

        
    }
}
