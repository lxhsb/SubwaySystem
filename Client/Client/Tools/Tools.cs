using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Web.Script.Serialization;
namespace Client.Tools
{
    class Tools
    {
        private Tools()
        {

        }
        public static string getMd5(string str)//等待修改最新的
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower();
        }

    }
}
