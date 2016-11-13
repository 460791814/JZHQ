using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL;
using System.Web.Caching;


namespace JZHQ.Handler
{
    /// <summary>
    /// GetCount 的摘要说明
    /// </summary>
    public class GetCount : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
         Cache cache=   HttpRuntime.Cache;
            context.Response.ContentType = "text/plain";
            T_Soft dal = new T_Soft();
           string method= context.Request["method"];
           if (method == "getsoftcount")
           {
               if (cache["getsoftcount"] == null)
               {
                   string count = dal.GetSoftCount();
                   cache.Insert("getsoftcount", count, null, DateTime.Now.AddSeconds(86400), TimeSpan.Zero);
               }

               context.Response.Write(cache["getsoftcount"]);
           }
           else if(method=="getzuoricount") {

               if (cache["getzuoricount"] == null)
               {
                   string count = dal.GetSoftCountZuoRi();
                   cache.Insert("getzuoricount", count, null, DateTime.Now.AddSeconds(86400), TimeSpan.Zero);
               }

               context.Response.Write(cache["getzuoricount"]);
           }
           else if (method == "jinri")
           {
               if (cache["jinri"] == null)
               {
                   string count = dal.GetSoftCountJinRi();
                   cache.Insert("jinri", count, null, DateTime.Now.AddSeconds(1800), TimeSpan.Zero);
               }


               context.Response.Write(cache["jinri"]);
           }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}