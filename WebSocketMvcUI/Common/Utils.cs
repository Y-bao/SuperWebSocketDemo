using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace WebSocketMvcUI.Common
{
    public class Utils
    {

        private static object obj = new object();
        /// <summary>
        /// 写日志，方便测试（看网站需求，也可以改成把记录存入数据库）
        /// </summary>
        /// <param name="sWord">要写入日志里的文本内容</param>
        public static void LogResult(string sWord)
        {

            sWord = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss") + sWord + "\r\n";
            //创建路径 
            string upLoadPath = HttpContext.Current.Server.MapPath("~/log/");
            if (!System.IO.Directory.Exists(upLoadPath))
            {
                System.IO.Directory.CreateDirectory(upLoadPath);
            }
            lock (obj)
            {
                //创建文件 写入错误 
                System.IO.File.AppendAllText(upLoadPath + DateTime.Now.ToString("yyyy.MM.dd") + ".log", sWord, System.Text.Encoding.UTF8);
            }

        }
    }
}