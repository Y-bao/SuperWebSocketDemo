using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperWebSocket;

namespace WebSocketDemoService
{
    public class MyDemoSession : WebSocketSession<MyDemoSession>
    {
        private string _Tmark = "";
        private string _MyGroupName = "";
        public string Tmark {
            get { return _Tmark; }
            set { _Tmark = value; }
        }
        public string MyGroupName {
            get { return _MyGroupName; }
            set { _MyGroupName = value; }
        }

    }
}
