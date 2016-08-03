using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WebSocketMvcUI.BLL;
using WebSocketMvcUI.Common;
using WebSocketMvcUI.WebSocketServer;

namespace WebSocketMvcUI.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }


        public JsonResult GetOnLineList()
        {
            JsonResult result = new JsonResult();
            BLLBase<zkpt_mm_trojanInfo> BLL = new BLLBase<zkpt_mm_trojanInfo>();

            DataTable dt = BLL.GetAllTableBy(zkpt_mm_trojanInfo._.userId == 1);
            result.Data = JsonConvert.SerializeObject(dt, new DataTableConverter()); 
            
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }


        public string ShowMenu(int id)
        {
            string Result = "Down";
            BLLBase<zkpt_mm_trojanInfo> BLL = new BLLBase<zkpt_mm_trojanInfo>();
            zkpt_mm_trojanInfo trojanModel = BLL.GetModel(zkpt_mm_trojanInfo._.tId == id);
            if (trojanModel!=null)
            {
                Result = "../Home/MainMenu/"+id;
            }

            return Result;
        
        }

        public ActionResult MainMenu(int id)
        {
            BLLBase<zkpt_mm_trojanInfo> BLL = new BLLBase<zkpt_mm_trojanInfo>();
            zkpt_mm_trojanInfo trojanModel = BLL.GetModel(zkpt_mm_trojanInfo._.tId == id);
            ViewData["tmark"] = trojanModel.tmark;
            return View();
        }

        public string SendOrderByWebSocketServer()
        { 
            BLLBase<zkpt_mm_Order> bll=new BLLBase<zkpt_mm_Order>();
            zkpt_mm_Order model=bll.GetModel(zkpt_mm_Order._.Tmark=="8083067");

            var list = WebSocketServer.WebSocketServer.GetServer.GetAllSessions();
            foreach (MyDemoSession item in list)
            {
                item.Send("返回的服务器信息：win7<br/>开机时间：2016");
            }
            return "ok";
        }

    }
}
