using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PageUI;

namespace JZHQ
{
    public partial class UpLoadList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            PageHtml page = new PageHtml();
            page.SearchUploadListTop100();
        }
    }
}