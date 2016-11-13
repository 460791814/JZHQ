using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL;

namespace JZHQ.Handler
{
    /// <summary>
    /// HitSoft 的摘要说明
    /// </summary>
    public class HitSoft : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            T_Soft dal = new T_Soft();
            string hash = context.Request["Hash"];
            if (!string.IsNullOrEmpty(hash))
            {
                dal.UpdateHitForSoft(hash);
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