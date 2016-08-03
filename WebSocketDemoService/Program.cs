using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using SuperWebSocket;
using WebSocketMvcUI.BLL;

namespace WebSocketDemoService
{
    class Program
    {
        static MyDemoService wsServer;
        static void Main(string[] args)
        {
            wsServer = new MyDemoService();
            
            if(!wsServer.Setup("192.168.11.234", 2012))
            {
                //设置IP 与 端口失败  通常是IP 和端口范围不对引起的 IPV4 IPV6
            }

            if (!wsServer.Start())
            {
                //开启服务失败 基本上是端口被占用或者被 某杀毒软件拦截造成的
                return;
            }

            wsServer.NewSessionConnected += (session) =>
            {
                //有新的连接
                Console.WriteLine("有新的连接"+session.Origin+"路径："+session.Path);
                session.Tmark = session.Path;
                foreach (MyDemoSession item in wsServer.GetAllSessions())
                {
                    item.Send(session.Tmark + "上线");
                }
            };
            wsServer.SessionClosed += (session, reason) =>
            {
                //有断开的连接
                Console.WriteLine("断开连接" + session.Origin + "路径：" + session.Path);
                foreach (MyDemoSession item in wsServer.GetAllSessions())
                {
                    item.Send(session.Tmark + "下线");
                }
            };
            wsServer.NewMessageReceived += (session, message) =>
            {
                //接收到新的文本消息
                Console.WriteLine("收到文本" + session.Origin + "路径：" + session.Path+"信息："+message);
                SaveOrder(message, session.SessionID);
                
            };
            wsServer.NewDataReceived += (session, bytes) =>
            {
                //接收到新的二进制消息
                Console.WriteLine("收到二进制信息" + session.Origin + "路径：" + session.Path + "信息：" + bytes.ToString());
            };

           
            
            Console.ReadKey();

            wsServer.Stop();
        }

        public static void SaveOrder(string Msg,string SessionID)
        {
            string[] msg = Msg.Split('|');
            if (msg.Length==3&&msg[0]=="01")
            {
                BLLBase<zkpt_mm_Order> bll = new BLLBase<zkpt_mm_Order>();
                bll.Insert(new zkpt_mm_Order()
                {
                    IsHandle=0,
                    StrOrder=msg[2],
                    StrRandom=SessionID,
                    Tmark=msg[1]
                });
            }
        }

        public void SendBack(string sessionid, string order)
        {
            wsServer.GetAppSessionByID(sessionid).Send(order);
        }

        
    }
}
