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
   public  class CollectData
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


               string str = HttpTool.GetHtml("http://bt.shousibaocai.com/search/" + KeyWord + "/" + CurrentPage, new CookieContainer());
          
              string xx="<li><p class=\"m-title\"><a href=\"/hash/([\\s\\S]*?)\">([\\s\\S]*?)</a></p><div class=\"m-files\"><ul class=\"m-files-ul\">([\\s\\S]*?)</ul></div><p class=\"m-meta\">([\\s\\S]*?)</p></li>";
        
           str = str.Replace("\n", "");
           str = str.Replace("&nbsp;", "");
           Regex r = new Regex(xx);
           if (r.IsMatch(str))
           {
               var ec = r.Matches(str);
               foreach (Match item in ec)
               {
                   E_Soft lieSoft = new E_Soft();
                   lieSoft.Hash = item.Groups[1].Value;
                   lieSoft.Name = item.Groups[2].Value;
                   lieSoft.URL = item.Groups[1].Value+"-1";
                   lieSoft.Details = GetLiContent(item.Groups[3].Value);
                   string sx = item.Groups[4].Value;
                   lieSoft.UpdateTime = Convert.ToDateTime((sx.Substring(sx.IndexOf("创建时间:"), sx.IndexOf("文件数") - sx.IndexOf("创建时间:"))).Replace("创建时间:", ""));
                   lieSoft.Length = (sx.Substring(sx.IndexOf("大小:"), sx.IndexOf("热度") - sx.IndexOf("大小:"))).Replace("大小:", "");
                   lieSoft.FileCount = int.Parse((sx.Substring(sx.IndexOf("文件数:"), sx.IndexOf("大小") - sx.IndexOf("文件数:"))).Replace("文件数:", ""));
                   eSoftList.Add(lieSoft);
               }
           }
          GetPage(str);
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
       public void GetPage(string str)
       {
           string count = "";
           List<string> list = new List<string>();
           string reg = "<div class=\"pagination\">(.*)</div>";
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
               KeyWord = list[0].Split('&')[0].Replace("?s=","");
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
