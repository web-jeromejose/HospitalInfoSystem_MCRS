/* Addded by : FHD 
 * Date : August 15, 2015
 * Desctiption:
 *  - Created to prevent direct link "HOT LINK/HOT LINKING" access of Features.
*  - What is does is it will take the module id, user id and use it to authenticate. It will return weather the user has
 *    access right to the feature.
 *  - Incase it will return false ( which means no access ) then the user swill be redirect to an error page.
 *    Hence, he will proceed to the feature.
 *  - This must be placed in your every controller. So that it will authenticate every request made.
*/
using DataLayer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using System.Web.Security;

namespace HIS.Controllers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class IsSGHFeatureAuthorizedAttribute : AuthorizeAttribute
    {
        public string mFeatureID { get; set; }
        public string mModuleID { get; set; }
        public string mUserID { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.Request.IsAuthenticated)
                return false;
            HISSecurity cs = new HISSecurity();
            FormsIdentity id = (FormsIdentity)httpContext.User.Identity;
            FormsAuthenticationTicket ticket = id.Ticket;   
            var d = ticket.UserData.Split('|');
            ConstantModel cons = new ConstantModel();

            mUserID = d[0].ToString();

            if (cs.IsFeatureAuthorized(mFeatureID, mUserID,cons.cModuleID ))
            {
                return true;

            }
            else
            {
                return false;
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                filterContext.Result = new HttpStatusCodeResult(403, "Sorry, you do not have the required permission to access this page.");

            }
            else
            {
                var viewResult = new ViewResult();

                viewResult.ViewName = "~/Views/Shared/Errors/_UnAuthorized.cshtml";
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                filterContext.HttpContext.Response.StatusCode = 403;
                filterContext.Result = viewResult;
            }
        }
    }
}
