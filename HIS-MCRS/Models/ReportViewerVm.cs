using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Models
{
    public class ReportViewerVm
    {
         public string Name { get; set; }
         public ReportViewer ReportViewer { get; set; }
       
    }
}