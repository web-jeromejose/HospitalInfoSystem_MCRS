using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HIS_MCRS.Areas.ManagementReports.Reports
{
    public partial class PrintPreview : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "application/pdf";// set the MIME type here
            Response.AddHeader("content-disposition", "attachment; filename=Test.PDF");
            Response.BinaryWrite((byte[])Session["rpt"]);
        }
    }
}