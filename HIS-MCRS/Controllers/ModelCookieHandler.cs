using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Mvc;

namespace HIS.Controllers
{
    public static class ModelCookieHandler
    {
        public static void StoreModelToCookies<T>(this ControllerContext context, string cookieName, T newModel) where T : class
        {
            var serializer = new JavaScriptSerializer();
            HttpCookie cookie;
            T savedModel;
            if (context.HttpContext.Request.Cookies[cookieName] == null)
            {
                cookie = new HttpCookie(cookieName);
                savedModel = newModel;
            }
            else
            {
                cookie = context.HttpContext.Request.Cookies[cookieName];
                savedModel = serializer.Deserialize<T>(cookie.Value);
                Type type = typeof(T);
                foreach (System.Reflection.PropertyInfo pi in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                {
                    object currentValue = type.GetProperty(pi.Name).GetValue(savedModel, null);
                    object newValue = type.GetProperty(pi.Name).GetValue(newModel, null);

                    if (currentValue != newValue && (currentValue == null || !currentValue.Equals(newValue)))
                    {
                        type.GetProperty(pi.Name).SetValue(savedModel, newValue, null);
                    }
                }
            }
            cookie.Value = serializer.Serialize(savedModel);
            context.HttpContext.Response.Cookies.Add(cookie);
        }
    }
}