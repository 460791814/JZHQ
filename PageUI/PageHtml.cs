using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibraryTool;
using BLL;
using Model;
using System.Web;
using ScTool;
using System.Net;
using Lucene.Net.Analysis.PanGu;
using System.Configuration;
using System.Reflection;
using System.Data;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace PageUI
{
    public class PageHtml
    {
        T_Soft tSoft = new T_Soft();
        Handel handel = new Handel();
        DBUtil db = new DBUtil();
        LuceneNetTool lucene = new LuceneNetTool();
        public string isluncen = ConfigurationManager.AppSettings["IsLuncen"];
        public string isMySql = ConfigurationManager.AppSettings["IsMySql"];
        public string error = "<div class='norecord'>" + ConfigurationManager.AppSettings["Error"] + "</div>";
        public void Default()
        {
            Template Tmplate = new Template("Index.htm");
            Utils.Write(Tmplate.HTML);
            Tmplate.Dispose();
        }

        public void SearchSoftList()
        {
            string fliter = ConfigurationManager.AppSettings["fliter"];
          
            //Safe.SafePrint();
            if (true)
            {
                string keyWord = HttpContext.Current.Server.UrlDecode(Utils.Request("KeyWord")); //System.Web.HttpUtility.UrlDecode(Utils.Request("KeyWord"));
              
                Utils.SaveLog(keyWord);
                int currentPage = Utils.Request("PageIndex") == null ? 1 : Convert.ToInt32(Utils.Request("PageIndex"));
                string method = Utils.Request("Method");
                int pageCount = 0;

                int pageSize = 10;
                int total = 0;
                Template Tmplate = new Template("Search.htm");
                Tmplate.KeyWord = keyWord;
                string LoopTemplate = Tmplate.GetPartContent("<!--<TemplateBegin>", "<TemplateEnd>-->");
                string content = "";

                    List<E_Soft> list = null;
                    if (isluncen == "1")
                    {
                        list = handel.PageQuery(keyWord, pageSize, currentPage, ref total);
                    }
                       // else if(isMySql=="1"){

                    //    T_Soft_MySql tsoftMySql = new T_Soft_MySql();
                    //    E_Soft eSoft = new E_Soft();
                    //    eSoft.KeyWord = keyWord;
                    //    eSoft.CurrentPage = currentPage;
                    //    eSoft.PageSize = pageSize;
                    //   list= tsoftMySql.SelectSoft(eSoft, ref total);
                    //}
                    else
                    {
                        E_Soft eSoft = new E_Soft();
                        eSoft.KeyWord = keyWord;
                        eSoft.CurrentPage = currentPage;
                        eSoft.PageSize = pageSize;

                        list = tSoft.SelectSoft(eSoft, ref total);
                    }
                    content = Tmplate.GetLoopContent(list, LoopTemplate);
                    if (string.IsNullOrEmpty(content) && !string.IsNullOrEmpty(keyWord)) //用户没有搜索到内容
                    {
                      

                        if (string.IsNullOrEmpty(content))
                        {
                            content = error;
                            Tmplate.Replace("{$ShowPage}", "");
                        }
                  

                    }
                    else
                    {


                        string pageLink = "/list/" + keyWord;
                        pageCount = total / pageSize;
                        if ((total % pageSize) > 0) pageCount++;
                        if (currentPage > pageCount) currentPage = pageCount;
                        content = Tmplate.GetLoopContent(list, LoopTemplate);
                        Tmplate.Replace("{$ShowPage}", handel.ShowPageV(pageCount, pageSize, currentPage, pageLink));
                        if (string.IsNullOrEmpty(content))
                        {
                            content = error;
                        }

                    }


                    if (fliter.Contains(keyWord))
                    {
                        content = error;
                        Tmplate.Replace("{content}", content);
                        Tmplate.Replace("{keyword}", keyWord);
                        Tmplate.Replace("{title}", "");
                        Tmplate.Replace("{currentpage}", currentPage.ToString());
                        Tmplate.Replace("{total}", total.ToString());
                        Utils.Write(Tmplate.HTML);
                        Tmplate.Dispose();
                    }
                    else
                    {

                        Tmplate.Replace("{content}", content);
                        Tmplate.Replace("{keyword}", keyWord);
                        Tmplate.Replace("{title}", keyWord + "第" + currentPage + "页");
                        Tmplate.Replace("{currentpage}", currentPage.ToString());
                        Tmplate.Replace("{total}", total.ToString());
                        Utils.Write(Tmplate.HTML);
                        Tmplate.Dispose();
                    }
            }
        }

        public void SearchSoftListTop100()
        {
          //  if (tSoft.GetKey())
            if(true)
            {
                string keyWord = HttpContext.Current.Server.UrlDecode(Utils.Request("KeyWord")); //System.Web.HttpUtility.UrlDecode(Utils.Request("KeyWord"));
                Utils.SaveLog(keyWord);
                int currentPage = Utils.Request("PageIndex") == null ? 1 : Convert.ToInt32(Utils.Request("PageIndex"));
                string method = Utils.Request("Method");
                string count = Utils.Request("Count");
                if (string.IsNullOrEmpty( count))
                {
                    count = "100";
                 }
                int pageCount = 0;
                int pageSize = 200;
                int total = 0;
                Template Tmplate = new Template("Top.htm");

                string LoopTemplate = Tmplate.GetPartContent("<!--<TemplateBegin>", "<TemplateEnd>-->");
                string content = "";


                List<E_Soft> list = null;
                if (isMySql == "1")
                {

                    T_Soft_MySql tsoftMySql = new T_Soft_MySql();

                    list = tsoftMySql.SelectSoftTop100(count);
                }
                else {
                    list = tSoft.SelectSoftTop100(count);
                }
                content = Tmplate.GetLoopContent(list, LoopTemplate);
                if (string.IsNullOrEmpty(content) && !string.IsNullOrEmpty(keyWord)) //用户没有搜索到内容
                {
                  
                    if (string.IsNullOrEmpty(content))
                    {
                        content = error;
                        Tmplate.Replace("{$ShowPage}", "");
                    }
                 

                }
                else
                {


                    string pageLink = "/list/" + keyWord;
                    pageCount = total / pageSize;
                    if ((total % pageSize) > 0) pageCount++;
                    if (currentPage > pageCount) currentPage = pageCount;
                    content = Tmplate.GetLoopContent(list, LoopTemplate);
                    Tmplate.Replace("{$ShowPage}", handel.ShowPageV(pageCount, pageSize, currentPage, pageLink));
                    if (string.IsNullOrEmpty(content))
                    {
                        content = error;
                    }

                }



                Tmplate.Replace("{content}", content);
                Tmplate.Replace("{keyword}", keyWord);
                Tmplate.Replace("{title}", keyWord + "第" + currentPage + "页");
                Tmplate.Replace("{currentpage}",  currentPage.ToString());

                //  Tmplate.ReplaceAllFlag();
                Utils.Write(Tmplate.HTML);
                Tmplate.Dispose();
            }
        }
        public void SearchUploadListTop100()
        {
            if (tSoft.GetKey())
            {
                string keyWord = HttpContext.Current.Server.UrlDecode(Utils.Request("KeyWord")); //System.Web.HttpUtility.UrlDecode(Utils.Request("KeyWord"));

                int currentPage = Utils.Request("PageIndex") == null ? 1 : Convert.ToInt32(Utils.Request("PageIndex"));
                string method = Utils.Request("Method");
                int pageCount = 0;
                int pageSize = 200;
                int total = 0;
                Template Tmplate = new Template("upload.htm");

                string LoopTemplate = Tmplate.GetPartContent("<!--<TemplateBegin>", "<TemplateEnd>-->");
                string content = "";


                List<E_Soft> list = tSoft.SelectUploadTop100();
                content = Tmplate.GetLoopContent(list, LoopTemplate);
                if (string.IsNullOrEmpty(content) && !string.IsNullOrEmpty(keyWord)) //用户没有搜索到内容
                {
                 
                    if (string.IsNullOrEmpty(content))
                    {
                        content = error;
                        Tmplate.Replace("{$ShowPage}", "");
                    }
                  

                }
                else
                {


                    string pageLink = "/list/" + keyWord;
                    pageCount = total / pageSize;
                    if ((total % pageSize) > 0) pageCount++;
                    if (currentPage > pageCount) currentPage = pageCount;
                    content = Tmplate.GetLoopContent(list, LoopTemplate);
                    Tmplate.Replace("{$ShowPage}", handel.ShowPageV(pageCount, pageSize, currentPage, pageLink));
                    if (string.IsNullOrEmpty(content))
                    {
                        content = error;
                    }

                }



                Tmplate.Replace("{content}", content);
                Tmplate.Replace("{keyword}", keyWord);
                Tmplate.Replace("{title}", keyWord + "第" + currentPage + "页");
      

              
                Utils.Write(Tmplate.HTML);
                Tmplate.Dispose();
            }
        }

        public string GetHash()
        {
            string str = Utils.Request("Hash");
            if (string.IsNullOrEmpty(str))
            {
                return "请上传种子文件";
            }
            else
            {
                return str;
            }
        }

        public void SoftInfo()
        {
          //  Safe.SafePrint();
            string hash = Utils.Request("Hash");

            if (string.IsNullOrEmpty(hash))
            {
                return;
            }

             tSoft.UpdateHitForSoft(hash);
             Template Tmplate = new Template("SoftInfo.htm");
             E_Soft list = null;
                E_Soft eSoft = new E_Soft();
                eSoft.Hash = hash;
                //if (isMySql == "1")
                //{

                //    T_Soft_MySql tsoftMySql = new T_Soft_MySql();

                //    list = tsoftMySql.SelectByHash(eSoft);
                //}
                //else
                //{
                     list = tSoft.SelectByHash(eSoft);
               // }

                StringBuilder intro = new StringBuilder();

         
                List<E_Soft> listXml = lucene.StrToXml(list.Details);
                string LoopTemplate = Tmplate.GetPartContent("<!--<TemplateBegin>", "<TemplateEnd>-->");
     
                string content = Tmplate.GetLoopContent(listXml, LoopTemplate);
                string softTypeName = "";
                if (list.SoftType == 2)
                {
                    softTypeName = "电影";
                }
                else if (list.SoftType == 1)
                {
                    softTypeName = "图片";
                }
                Tmplate.Replace("{softname}", list.Name);
                string fenci = handel.GetKeyWordsSplitBySpace(list.Name, new PanGuTokenizer());
                Tmplate.Replace("{fenci}", fenci);
                string xiangguan = "";
                if (!string.IsNullOrEmpty(fenci))
                {
                    if (fenci.Contains('|'))
                    {
                        var xxx = fenci.Split('|');
                        for (int i = 0; i < xxx.Length; i++)
                        {

                            xiangguan += "<a href='/list/" + xxx[i] + "/1'>" + xxx[i] + "</a>,";

                        }
                    }
                    else
                    {

                        xiangguan += "<a href='/list/" + fenci + "/1'>" + fenci + "</a>";
                    }

                }
                Tmplate.Replace("{xiangguan}", xiangguan);
                Tmplate.Replace("{softtype}", softTypeName);
                Tmplate.Replace("{length}", list.Length);
                Tmplate.Replace("{filecount}", list.FileCount.ToString());
                Tmplate.Replace("{updatetime}", list.UpdateTime.ToString("yyyy-MM-dd"));
                Tmplate.Replace("{hit}", list.Hit.ToString());
                Tmplate.Replace("{content}", content);


      

            //  Tmplate.Replace("{SoftType}", db.GetSoftTypeStr(list[0].SoftType));

            Tmplate.Replace("{hash}", hash);
            Tmplate.ReplaceAllFlag();
            Utils.Write(Tmplate.HTML);
            Tmplate.Dispose();
        }
        /// <summary>
        /// 生成索引
        /// </summary>
        public void CreateLucene()
        {

            //   lucene.CreateIndexByData(tSoft.SelectSoft(new E_Soft()));
        }
        public List<T> GetList<T>(DataTable table)
        {
            List<T> list = new List<T>();
            T t = default(T);
            PropertyInfo[] propertypes = null;
            string tempName = string.Empty;
            foreach (DataRow row in table.Rows)
            {
                t = Activator.CreateInstance<T>();
                propertypes = t.GetType().GetProperties();
                foreach (PropertyInfo pro in propertypes)
                {
                    tempName = pro.Name;
                    if (table.Columns.Contains(tempName))
                    {
                        object value = row[tempName];
                        if (value.GetType() == typeof(System.DBNull))
                        {
                            value = null;
                        }
                        pro.SetValue(t, value, null);
                    }
                }
                list.Add(t);
            }
            return list;
        }

        public void API()
        {
            string method = Utils.Request("method");
            if (method == "get_list")
            {
                string keyWord = HttpContext.Current.Server.UrlDecode(Utils.Request("keyword")); //System.Web.HttpUtility.UrlDecode(Utils.Request("KeyWord"));

                Utils.SaveLog(keyWord);
                int currentPage = Utils.Request("pageindex") == null ? 1 : Convert.ToInt32(Utils.Request("pageindex"));

                int pageCount = 0;

                int pageSize = Utils.Request("pagesize") == null ? 10 : Convert.ToInt32(Utils.Request("pagesize")); ;
                int total = 0;

                List<E_Soft> list = null;
                if (isluncen == "1")
                {
                    list = handel.PageQuery(keyWord, pageSize, currentPage, ref total);
                }
                else
                {
                    E_Soft eSoft = new E_Soft();
                    eSoft.KeyWord = keyWord;
                    eSoft.CurrentPage = currentPage;
                    eSoft.PageSize = pageSize;

                    list = tSoft.SelectSoft(eSoft, ref total);
                }
                Utils.Write(JsonConvert.SerializeObject(list),true);
            }
            if (method == "get_info")
            {
            
                string hash = Utils.Request("hash");

                if (string.IsNullOrEmpty(hash))
                {
                    return;
                }

             
                E_Soft list = null;
                E_Soft eSoft = new E_Soft();
                eSoft.Hash = hash;
             
                list = tSoft.SelectByHash(eSoft);
                Utils.Write(JsonConvert.SerializeObject(list), true);

              
            }
         
        }


    }
}
