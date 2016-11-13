using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UMULib;

namespace JZHQ
{
    public partial class HashToBt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string action = Request["Action"];
            string hash = Request["Hash"];
            if (action == "GetKey")
            {
                UrlGenerator U = new UrlGenerator();
                string key = U.GenBitCometTorrentKey(hash);
                Context.Response.Write(key);
                Context.Response.End();
            }else
            if (action == "GetURL")
            {
                UrlGenerator U = new UrlGenerator();
                string key = U.GenBitCometTorrentKey(hash);
                Context.Response.Redirect("http://torrent-cache.bitcomet.org:36869/get_torrent?info_hash=" + hash + "&key=" + key );
              
            }
        }
    }
}