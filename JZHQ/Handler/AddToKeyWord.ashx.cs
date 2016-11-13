using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL;
using Model;

namespace JZHQ.Handler
{
    /// <summary>
    /// AddToKeyWord 的摘要说明
    /// </summary>
    public class AddToKeyWord : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
           
          string keyWord= HttpContext.Current.Server.UrlDecode(context.Request["KeyWord"]);
          if (!string.IsNullOrEmpty(keyWord))
          {
              T_Soft dal = new T_Soft();
              E_KeyWord ekey = new E_KeyWord();
              ekey.KeyWord = keyWord;
              ekey.Hit = 1;
              ekey.IsSearch = false;
              ekey.UpdateTime = DateTime.Now;
              if (dal.SelectKeyWord(ekey))  //存在关键词
              {
                  dal.UpdateHitForKeyWord(ekey);
              }
              else {
                  dal.InsertInToKeyWord(ekey);
              }
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