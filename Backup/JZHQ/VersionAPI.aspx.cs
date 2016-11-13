using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Configuration;
using Newtonsoft.Json;

namespace JZHQ
{
    public partial class VersionAPI : System.Web.UI.Page
    {
        public string Version;
        public string DownString;
        protected void Page_Load(object sender, EventArgs e)
        {
            string versionPath = HttpContext.Current.Server.MapPath("version.txt");
            string downPath = HttpContext.Current.Server.MapPath("down.txt");
            if (this.Request["method"] == "update")
            {
                StreamWriter writer = new StreamWriter(versionPath, false);
                StreamWriter down = new StreamWriter(downPath, false);
                writer.WriteLine(Request.Form["version"]);
                writer.Dispose();
                down.WriteLine(Request.Form["down"]);
                down.Dispose();
                Response.Write("修改成功，刷新本页URL地址即可看到效果");
                Response.End();
            }
            else if (Request["method"] == "getversion")
            {
                Result r = new Result();
                //打开文件
                StreamReader reader = new StreamReader(versionPath, Encoding.GetEncoding("gb2312"));
                //读取流
                string versionContent = reader.ReadLine();
                reader.Dispose();
                string str=Request["version"];
                string downContent = "";
                if (str != versionContent)
                {
                    StreamReader down = new StreamReader(downPath, Encoding.GetEncoding("gb2312"));
                    //读取流
                    downContent = down.ReadLine();
                    down.Dispose();
                    r.status = false;
                    r.message = downContent;
                }
                else {
                    r.status = true;
                }
                Response.Write(JsonConvert.SerializeObject(r)); 
                Response.End();
            }
            else
            {
           
                //打开文件
              StreamReader reader = new StreamReader(versionPath, Encoding.GetEncoding("gb2312"));
                //读取流
              Version = reader.ReadLine();
                reader.Dispose();
                StreamReader down = new StreamReader(downPath, Encoding.GetEncoding("gb2312"));
                //读取流
                DownString = down.ReadLine();
                down.Dispose();

            }
        }
        public class Result
        {
            public bool status;
            public string message;
        }
    }
}