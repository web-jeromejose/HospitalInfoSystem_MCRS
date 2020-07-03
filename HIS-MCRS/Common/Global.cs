using DataLayer.Data;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Common
{
    public static class Global
    {

        public static string ReportViewerSessionName = "reportViewerSession";
        public static string PdfUriSessionName = "pdffile";

        private static OrganizationDetails _organizationDetails;
        public static OrganizationDetails OrganizationDetails
        {
            get
            {
                if (_organizationDetails == null)
                {
                    OrganizationDetailDB db = new OrganizationDetailDB();

                    _organizationDetails = db.getOrganizationDetails();

                }
                return _organizationDetails;
            }
        }
    }
}