using DataLayer;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Web.Script.Serialization;
using System.Text;

namespace HIS_MCRS.Areas.Reports
{
    public partial class Reports : System.Web.UI.Page
    {
        public string Rdisp = "";
        public int rtype = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            /* REPORT LIST  
             * 1 - combine statement
             */
            Rdisp = Rdisp??Request.QueryString["rdisp"].ToString();
            int.TryParse(Request.QueryString["rtype"], out rtype);
            if (!IsPostBack)
            {
                PermissionSet permissions = new PermissionSet(PermissionState.Unrestricted);
                DBHelper DB = new DBHelper();
                DataTable dt = null;

                ReportViewer1.Reset();

                if (rtype == 1000) { //Act. Adj

                    int invtype = 0;
                    int catid = 0;
                    long comid = 0;
                    int filter = 0;

                    DateTime fdate = DateTime.MinValue;
                    DateTime tdate = DateTime.MinValue;

                    int.TryParse(Request.QueryString["invtype"], out invtype);
                    int.TryParse(Request.QueryString["catid"], out catid);
                    long.TryParse(Request.QueryString["comid"], out comid);
                    int.TryParse(Request.QueryString["filtertype"], out filter);

                    DateTime.TryParse(Request.QueryString["fdate"].ToString(), out fdate);
                    DateTime.TryParse(Request.QueryString["tdate"].ToString(), out tdate);

                    DB.param = new SqlParameter[]{
                        //new SqlParameter("@filter", filter),
                        new SqlParameter("@ptype", invtype),
                        new SqlParameter("@catid", catid),
                        new SqlParameter("@comid", comid),
                        new SqlParameter("@fdate", fdate),
                        new SqlParameter("@tdate", tdate),
                    };

                    dt = DB.ExecuteSPAndReturnDataTable("aropbilling.Get_actual_adjustment_data");
                    ReportViewer1.Reset();
                    ReportViewer1.LocalReport.ReportPath = "Areas/ManagementReports/Reports/ActAdjReports/ActualandAdjustedBills.rdl";
                    ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet", dt));
                    ReportViewer1.LocalReport.SetBasePermissionsForSandboxAppDomain(permissions);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter("filter", filter.ToString()));
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter("ptype", invtype.ToString()));
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter("catid", catid.ToString()));
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter("comid", comid.ToString()));
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter("fdate", fdate.ToString()));
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter("tdate", tdate.ToString()));

                    ReportViewer1.LocalReport.DataSources.Clear();
                    

                    ReportViewer1.DataBind();
                    ReportViewer1.LocalReport.Refresh();
                } 
            }
        }

        public class ipbillforarray
        {
            public long ipbillno { get; set; }
        }

        public string LocalComputerName()
        {
            string netBiosName = System.Environment.MachineName;
            string dnsName = System.Net.Dns.GetHostName();
            return dnsName;
        }
        public string LocalIPAddress()
        {
            string stringIpAddress;
            stringIpAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (stringIpAddress == null) //may be the HTTP_X_FORWARDED_FOR is null
                stringIpAddress = Request.ServerVariables["REMOTE_ADDR"]; //we can use REMOTE_ADDR
            else if (stringIpAddress == null)
                stringIpAddress = GetLanIPAddress();

            return stringIpAddress;
        }
        public string GetLanIPAddress()
        {
            //Get the Host Name
            string stringHostName = Dns.GetHostName();
            //Get The Ip Host Entry
            IPHostEntry ipHostEntries = Dns.GetHostEntry(stringHostName);
            //Get The Ip Address From The Ip Host Entry Address List
            System.Net.IPAddress[] arrIpAddress = ipHostEntries.AddressList;
            return arrIpAddress[arrIpAddress.Length - 1].ToString();
        }

        protected void ExcelExport_Click(object sender, EventArgs e)
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            string filename;

            byte[] bytes = ReportViewer1.LocalReport.Render(
               "Excel", null, out mimeType, out encoding,
                out extension,
               out streamids, out warnings);

            filename = string.Format("{0}.{1}", "ExportToExcel" + DateTime.Now.ToString("dd-MMM-yyyy-hh_mm_ss"), "xls");
            Response.ClearHeaders();
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            Response.ContentType = mimeType;
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }

        protected void Print_Click(object sender, EventArgs e)
        {
            if (Rdisp == "P")
            {
                print_A4_portrait();
            }
            else if (Rdisp == "L")
            {
                print_A4_landscape();
            }
        }

        /* will print A4 Portrait 8.27 x 11.69 */
        protected void print_A4_portrait() {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            string dtnow = DateTime.Now.ToString("ddMMMyyyy_hh_mm_ss");

            byte[] bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType,
                           out encoding, out extension, out streamids, out warnings);

            string fileOUT = HttpContext.Current.Server.MapPath(@"\Areas\ManagementReports\Reports\TempReport\" + "output_" + dtnow + "_" + rtype.ToString() + ".pdf");

            //if (!File.Exists(fileOUT))
            //{
            //    File.Create(fileOUT).Dispose();
            //}


            FileStream fs = new FileStream(fileOUT,
            FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
            Document document = new Document(PageSize.A4);
            //Open existing PDF
            PdfReader reader = new PdfReader(fileOUT);
            //Getting a instance of new PDF writer
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(
               HttpContext.Current.Server.MapPath(@"\Areas\ManagementReports\Reports\TempReport\" + "print_" + dtnow + "_" + rtype.ToString() + ".pdf"), FileMode.Create));
            document.Open();
            PdfContentByte cb = writer.DirectContent;

            int i = 0;
            int p = 0;
            int n = reader.NumberOfPages;
            Rectangle psize = reader.GetPageSize(1);

            float width = psize.Width;
            float height = psize.Height;

            //Add Page to new document
            while (i < n)
            {
                document.NewPage();
                p++;
                i++;

                PdfImportedPage page1 = writer.GetImportedPage(reader, i);
                cb.AddTemplate(page1, 0, 0);
            }

            //Attach javascript to the document
            PdfAction jAction = PdfAction.JavaScript("this.print(true);\r", writer);
            writer.AddJavaScript(jAction);
            document.Close();

            //Attach pdf to the iframe
            //frmPrint.Attributes["src"] = @"\Areas\ManagementReports\Reports\TempReport\" + "print_" + dtnow + "_" + rtype.ToString() + ".pdf";
        }
        /* will print A4 Landscape 11.69 x 8.27 */
        protected void print_A4_landscape() {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            string dtnow = DateTime.Now.ToString("ddMMMyyyy_hh_mm_ss");

            byte[] bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType,
                           out encoding, out extension, out streamids, out warnings);

            string fileOUT = HttpContext.Current.Server.MapPath(@"\Areas\ManagementReports\Reports\TempReport\" + "output_" + dtnow + "_" + rtype.ToString() + ".pdf");

            FileStream fs = new FileStream(fileOUT,
            FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
            Document document = new Document(PageSize.A4.Rotate());
            //Open existing PDF
            PdfReader reader = new PdfReader(fileOUT);
            //Getting a instance of new PDF writer
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(
              HttpContext.Current.Server.MapPath(@"\Areas\ManagementReports\Reports\TempReport\" + "print_" + dtnow + "_" + rtype.ToString() + ".pdf"), FileMode.Create));
            document.Open();
            PdfContentByte cb = writer.DirectContent;

            int i = 0;
            int p = 0;
            int n = reader.NumberOfPages;
            Rectangle psize = reader.GetPageSize(1);

            float width = psize.Width;
            float height = psize.Height;

            //Add Page to new document
            while (i < n)
            {
                document.NewPage();
                p++;
                i++;

                PdfImportedPage page1 = writer.GetImportedPage(reader, i);
                cb.AddTemplate(page1, 0, 0);
            }

            //Attach javascript to the document
            PdfAction jAction = PdfAction.JavaScript("this.print(true);\r", writer);
            writer.AddJavaScript(jAction);
            document.Close();

            //Attach pdf to the iframe
            //frmPrint.Attributes["src"] = @"\Areas\ManagementReports\Reports\TempReport\" + "print_" + dtnow + "_" + rtype.ToString() + ".pdf";
        }

        public void PrintToPDF_Click(object sender, EventArgs e) {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string filenameExtension;
            byte[] bytes;

            bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.ContentType = "Application/pdf";
            Response.AddHeader("content-length", bytes.Length.ToString());
            Response.BinaryWrite(bytes);

            Response.Flush();
            Response.Close();
            Response.End();

           // StringBuilder sb = new StringBuilder();

            //sb.Append("<script type='text/javascript'>");
            //sb.Append("windows.onload('alert('waawaw')')");
            //sb.Append("</script>");

            //sb.Append(bytes);

            //this.frmPrint.InnerText = "wawawawaw" + "<embed src='" + bytes.ToString() + "'  width='100%' height='300'></embed>";
            
            //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "", true);
            ClientScript.RegisterClientScriptBlock(this.GetType(),
                "Delay", "<script type='text/javascript'></script>");
        
            //this.frmPrint.InnerText = bytes.ToString();



            //HttpCookie cookie = new HttpCookie("myReport");
            //cookie.Value = Convert.ToBase64String(bytes);
            //Response.Cookies.Add(cookie);

            //toread

            //HttpCookie c = Request.Cookies["myCookie"];
            //byte[] retrievedByteArray = Convert.FromBase64String(c.Value);

            //HttpCookie c = Request.Cookies["myCookie"];
            //byte[] retrievedByteArray = System.Text.Encoding.UTF8.GetBytes(c.Value);


            //System.Text.StringBuilder _sb = new System.Text.StringBuilder();
            //_sb.Append("window.open('PrintPreview.aspx','',");
            //_sb.Append("'toolbar=0,menubar=0,resizable=yes')");
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "winOpen", _sb.ToString(), true);
        }
    }
}
