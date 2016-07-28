using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperWebSocket;

namespace WebSocketDemoService
{
    public class MyDemoSession : WebSocketSession<MyDemoSession>
    {
        private string _MyName = "";
        private string _MyGroupName = "";
        public string MyName {
            get { return _MyName; }
            set { _MyName = value; }
        }
        public string MyGroupName {
            get { return _MyGroupName; }
            set { _MyGroupName = value; }
        }

    }
}
