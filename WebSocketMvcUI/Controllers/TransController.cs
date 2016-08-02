using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSocketMvcUI.Controllers
{
    public class TransController : Controller
    {
        //
        // GET: /Trans/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetOrder(string Id)
        { 
            JsonResult result=new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;//允许使用GET方式获取，否则用GET获取是会报错
            result.Data = "[{name:" + Id + ",sex:男},{name:小芳,sex:女}]";
            return result;
        }

    }
}
