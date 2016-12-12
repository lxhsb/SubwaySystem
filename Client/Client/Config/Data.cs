using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Config
{
    class Data
    {
        private Data() { }
        private static string DefaultStation;
        private static string[][] RouteList = new string[3][];
        private static Dictionary<string, int> Station2Num = new Dictionary<string, int>();
        private static int StationCount;
        private static int[][] Distance = new int[2000][];//你最多不可能超过2000个站呗
        public static void InitData(string _DefaultStation)
        {
            DefaultStation = _DefaultStation;
            RouteList[1] = new string[] { "西流湖", "西三环", "秦岭路", "桐柏路", "碧沙岗", "绿城广场", "医学院", "郑州火车站", "二七广场", "人民路", "紫荆山", "燕庄", "民航路", "会展中心", "黄河南路", "农业南路", "东风南路", "郑州东站", "博学路", "市体育中心" };
            RouteList[2] = new string[] { "刘庄", "柳林", "沙门", "北三环", "东风路", "关虎屯", "黄河路", "紫荆山", "东大街", "陇海东路", "二里岗", "南五里堡", "花寨", "南三环", "战马屯", "南四环" };
            StationCount = 1;
            for(int i= 1;i<=2;i++)
            {
                for(int j= 0;j<RouteList[i].Length;j++)
                {
                    if (Station2Num.ContainsKey(RouteList[i][j]))
                        continue;
                    Station2Num.Add(RouteList[i][j], StationCount);
                    StationCount++;
                }
            }
            for (int i = 0; i <= StationCount+1; i++)
            {
                Distance[i] = new int[StationCount+1];
            }
            for(int i=0;i<=StationCount;i++)
            {
                for(int j = 0;j<=StationCount;j++)
                {
                    Distance[i][j] = 40000000;
                }
            }
            for(int i= 1;i<=2;i++)
            {
                for(int j= 0;j<RouteList[i].Length-1;j++)
                {
                    string st1 = RouteList[i][j];
                    string st2 = RouteList[i][j + 1];
                    int st1num = Station2Num[st1];
                    int st2num = Station2Num[st2];
                    Distance[st1num][st2num] = 1;
                    Distance[st2num][st1num] = 1;
                }
            }
            floyd();


        }
        private static void floyd()
        {
            for(int k= 1;k<StationCount;k++)
            {
                for(int j=1;j<StationCount;j++)
                {
                    for(int i=1;i<StationCount;i++)
                    {
                        if(Distance[i][j]<Distance[i][k]+Distance[k][j])
                        {
                            Distance[i][j] = Distance[i][k] + Distance[k][j];
                        }
                    }
                }
            }

        }
        public static int getDistance(string from ,string to)
        {
            int fromNum, toNum;
            if (Station2Num.TryGetValue(from, out fromNum) == false || Station2Num.TryGetValue(to, out toNum) == false)
                return -1;
            return Distance[fromNum][toNum];
        }
        public static void ResetDefaultStation(string _DefaultStation)
        {
            DefaultStation = _DefaultStation;
        }
        public static string[][] getAllStation()
        {
            return RouteList;
        }
        public static int getPrice(int dis)
        {
            if (dis < 4)
                return 2;
            else if (dis < 7)
                return 3;
            else if (dis < 11)
                return 4;
            else if (dis < 16)
                return 5;
            else
                return 6;

        }







    }
}