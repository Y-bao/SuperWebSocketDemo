using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;
using WebSocketMvcUI.BLL;

namespace WebSocketMvcUI.WebSocketServer
{
    public class WebSocketServer
    {
        private static MyDemoService _wsServer;
        private static object objlocker = new object();

        public static MyDemoService GetServer
        {
            get {

                if (_wsServer == null)
                {
                    lock (objlocker)
                    {
                        _wsServer = new MyDemoService();
                    }
                }
                return _wsServer;
            }
        }

        public static void StartServer()
        {
            _wsServer = GetServer;
            if (!_wsServer.Setup("192.168.11.234", 2012))
            {
                //设置IP 与 端口失败  通常是IP 和端口范围不对引起的 IPV4 IPV6
            }

            if (!_wsServer.Start())
            {
                //开启服务失败 基本上是端口被占用或者被 某杀毒软件拦截造成的
                return;
            }

            _wsServer.NewSessionConnected += (session) =>
            {
                //有新的连接
            };
            _wsServer.SessionClosed += (session, reason) =>
            {
                //有断开的连接
            };
            _wsServer.NewMessageReceived += (session, message) =>
            {
                //接收到新的文本消息
                Console.WriteLine("收到文本" + session.Origin + "路径：" + session.Path + "信息：" + message);
                SaveOrder(message, session.SessionID);
            };
        
        }

        public static void SaveOrder(string Msg, string SessionID)
        {
            string[] msg = Msg.Split('|');
            if (msg.Length == 3 && msg[0] == "01")
            {
                BLLBase<zkpt_mm_Order> bll = new BLLBase<zkpt_mm_Order>();
                bll.Insert(new zkpt_mm_Order()
                {
                    IsHandle = 0,
                    StrOrder = msg[2],
                    StrRandom = SessionID,
                    Tmark = msg[1]
                });
            }
        }

        public static void SendBack(string sessionid, string order)
        {
            _wsServer.GetAppSessionByID(sessionid).Send(order);
        }


    }
}