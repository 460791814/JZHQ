<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;

public class Handler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        HttpPostedFile file = context.Request.Files[0];
        string sPath = HttpContext.Current.Server.MapPath("/File/" );
        if (System.IO.Directory.Exists(sPath) == false) System.IO.Directory.CreateDirectory(sPath);
        if (System.IO.Path.GetExtension(file.FileName).ToLower() == ".torrent")
        {
        string filePath = sPath + file.FileName;
        file.SaveAs(filePath);
      
        JZHQ.Handler.Torrent t = new JZHQ.Handler.Torrent(filePath);
        byte[] b = t.InfoHash;
      //  string xx = t.Name;
        
        JZHQ.Handler.hashTool sha1 = new JZHQ.Handler.hashTool();
        string hash = sha1.ByteArrayToHexString(b);
        BLL.T_Soft tSoft = new BLL.T_Soft();
        Model.E_Soft eSoft = new Model.E_Soft();
        eSoft.Hash = hash;
        eSoft.Name = file.FileName;
        tSoft.InsertInToUpload(eSoft);
        context.Response.Redirect("/UploadBT.aspx?Hash=magnet:?xt=urn:btih:" + hash);
        context.Response.Write("保存成功");
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }
    

}