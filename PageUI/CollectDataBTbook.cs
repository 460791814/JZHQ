using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using ScTool;
using Model;

namespace PageUI
{
   public  class CollectDataBTbook
    {
       /// <summary>
       /// 
       /// </summary>
       public string DataType
       {
           get;
           set;
       }
       /// <summary>
       /// 当前页
       /// </summary>
       public int CurrentPage
       {
           get;
           set;
       }
       /// <summary>
       ///页数
       /// </summary>
       public int PageCount;
       public List<E_Soft> eSoftList;
       public string KeyWord;
       public string Hash;
       public E_Soft eSoft;
     
       public void GetContent()
       {
           eSoftList = new List<E_Soft>();
           try
           {


               string str = HttpTool.GetHtml("http://h31bt.com/search/" + KeyWord + "/" + CurrentPage+".html", new CookieContainer());
          
              string xx="<div class=\"search-item\"><div class=\"item-title\"><a href=\"/detail/([\\s\\S]*?)\" title=\"([\\s\\S]*?)\" target=\"_blank\">([\\s\\S]*?)</a></div><div class=\"item-list\"><p>([\\s\\S]*?)</p></div><div class=\"item-bar\"><span>创建时间：<b>([\\s\\S]*?)</b></span><span>大小：<b>([\\s\\S]*?)</b></span><span>文件数量：<b>([\\s\\S]*?)个</b></span>([\\s\\S]*?)</div></div>";
        
           str = str.Replace("\n", "");
           Regex r = new Regex(xx);
           if (r.IsMatch(str))
           {
               var ec = r.Matches(str);
               foreach (Match item in ec)
               {
                   E_Soft lieSoft = new E_Soft();
                   lieSoft.Hash = item.Groups[1].Value;
                   lieSoft.Name = item.Groups[2].Value;
                   lieSoft.URL = item.Groups[1].Value+"-2";
                   lieSoft.Details = item.Groups[4].Value;
                   lieSoft.UpdateTime =Convert.ToDateTime(item.Groups[5].Value);
                   lieSoft.Length = item.Groups[6].Value;
                   lieSoft.FileCount =int.Parse( item.Groups[7].Value);
                   eSoftList.Add(lieSoft);
               }
           }
          GetPage(str);
          GetKeyWord(str);
           }
           catch (Exception)
           {

               eSoftList = null;
               PageCount = 0;
           }
       }
       public string GetLiContent(string str)
       {
           StringBuilder liContent = new StringBuilder();
           string reg = "<li>([\\s\\S]*?)<span style=\"color:#888;\">([\\s\\S]*?)</span></li>";
           str = str.Replace("\n", "");
           Regex r = new Regex(reg);
           if (r.IsMatch(str))
           {
               var ec = r.Matches(str);
               foreach (Match item in ec)
               {
                   liContent.Append(item.Groups[1].Value + "&nbsp&nbsp&nbsp&nbsp&nbsp" + item.Groups[2].Value+"<br/>");
               }
           }
           return liContent.ToString() ;
       }

       public void GetKeyWord(string str)
       {
           StringBuilder liContent = new StringBuilder();
           string reg = "<input type=\"text\" id=\"search\" title=\"Search\" value=\"([\\s\\S]*?)\" autocomplete=\"off\" name=\"q\" maxlength=\"50\">";
           str = str.Replace("\n", "");
           Regex r = new Regex(reg);
           if (r.IsMatch(str))
           {
               var ec = r.Matches(str);
               foreach (Match item in ec)
               {
                   KeyWord = item.Groups[1].Value;
               }
           }
          
       }

       public void GetPage(string str)
       {
           string count = "";
           List<string> list = new List<string>();
           string reg = "<div class=\"bottom-pager\">(.*)</div>";
           str = str.Replace("\n", "");
           Regex r = new Regex(reg);
           if (r.IsMatch(str))
           {
               Match mat = r.Match(str);
               string pageStr = mat.Groups[1].Value;

               string aReg = "<a href=\"/?([\\s\\S]*?)\">([\\s\\S]*?)</a>";

               Regex ar = new Regex(aReg);
               if (ar.IsMatch(pageStr))
               {
                   var aec = ar.Matches(pageStr);
                   foreach (Match item in aec)
                   {
                       if (!list.Contains(item.Groups[1].Value))
                       {
                           list.Add(item.Groups[1].Value);
                           count = item.Groups[2].Value;
                       }
                   }
               }
           }
          
           if (list.Count>0)
           {
              // KeyWord = list[0].Split('&')[0].Replace("?s=","");
               PageCount = Convert.ToInt32(count.Replace("[", "").Replace("]", ""));
           }
       }
        #region 抓取详情页的信息
       public void GetInfo()
       {
           eSoft = new E_Soft();
           string str = HttpTool.GetHtml("http://bt.shousibaocai.com/hash/" + Hash.ToLower() , new CookieContainer());
           string reg = "<h3>([\\s\\S]*?)</h3>";
           str = str.Replace("\n", "");
           Regex r = new Regex(reg);
           if (r.IsMatch(str))
           {
               Match mat = r.Match(str);
               eSoft.Name = mat.Groups[1].Value;
           }
           GetInfoLi(str);
       }
       public void GetInfoLi(string str)
       {
           StringBuilder liContent = new StringBuilder();
           string reg = "<ol style=\"font-size: 14px;\">([\\s\\S]*?)</ol>";
           Regex r = new Regex(reg);
           if (r.IsMatch(str))
           {
               Match mat = r.Match(str);
             string liStr=  mat.Groups[1].Value;
               string regLi = "<li>([\\s\\S]*?)<span style=\"color:#888\">([\\s\\S]*?)</span></li>";
               str = str.Replace("\n", "");
               Regex rLi = new Regex(regLi);
               if (rLi.IsMatch(liStr))
               {
                   var ec = rLi.Matches(liStr);
                   for (int i=0; i < ec.Count;i++ )
                   {
                       Match item = ec[i];
                       //liContent.Append(item.Groups[1].Value + "&nbsp&nbsp&nbsp&nbsp&nbsp" + item.Groups[2].Value + "<br/>");

                       if (i == ec.Count - 1)
                       {
                           liContent.Append("<li class='last'>" + item.Groups[1].Value + "<small>" + item.Groups[2].Value + "</small></li>");
                       }
                       else
                       {

                           liContent.Append("<li>" + item.Groups[1].Value + "<small>" + item.Groups[2].Value + "</small></li>");
                       }
                   }
               }
           }
           eSoft.Details = liContent.ToString();
       }
        #endregion
    }
}
