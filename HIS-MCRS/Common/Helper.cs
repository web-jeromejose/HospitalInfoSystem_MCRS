using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace HIS_MCRS.Common
{
    public static class Helper
    {
        //vb val function in c#
        public static Double Val(string value)
        {
            String result = String.Empty;
            foreach (char c in value)
            {
                if (Char.IsNumber(c) || (c.Equals('.') && result.Count(x => x.Equals('.')) == 0))
                    result += c;
                else if (!c.Equals(' '))
                    return String.IsNullOrEmpty(result) ? 0 : Convert.ToDouble(result);
            }
            return String.IsNullOrEmpty(result) ? 0 : Convert.ToDouble(result);
        }

        public static MemoryStream createFileMemoryStream(ReportViewer reportViewer, string format)
        {
            MemoryStream memoryStream = new MemoryStream();
           
            try
            {
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string filenameExtension;

                byte[] _bytes = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

                memoryStream = new MemoryStream(_bytes);
                memoryStream.Position = 0;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }

            return memoryStream;
        }

        public static string getApplicationUri(string action, string controller, string area)
        {
           StringBuilder uribuilder = new StringBuilder();

            string sAuthority = System.Web.HttpContext.Current.Request.Url.Authority;
            string sApplicationPath = System.Web.HttpContext.Current.Request.ApplicationPath;
            sApplicationPath = sApplicationPath == "/" ? "" : sApplicationPath;

            uribuilder.Append(sAuthority + sApplicationPath);
            
            if (!string.IsNullOrEmpty(area))
            {
                uribuilder.Append("/" + area );
            }

            uribuilder.Append("/" + controller);
            uribuilder.Append("/" + action);
            return uribuilder.ToString();
        }


        public static MemoryStream MergeMemoryStream(MemoryStream stream1, MemoryStream stream2)
        {
            stream2.CopyTo(stream1);
            //stream1 holds the combined stream
            return  stream1; 
        }
    }
}