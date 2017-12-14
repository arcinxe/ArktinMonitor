using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using ArktinMonitor.Data;
using ArktinMonitor.Data.Models;

namespace ArktinMonitor.WebApp.Providers
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class SwitchableAuthorizationAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var db = new ArktinMonitorDataAccess();
            db.DebugLogs.Add(new DebugLog() { Message = nameof(OnAuthorization), TimeStamp = DateTime.Now });
            db.SaveChanges();
            base.OnAuthorization(filterContext);
        }

        protected override HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
        {
            var db = new ArktinMonitorDataAccess();
            db.DebugLogs.Add(new DebugLog() { Message = nameof(OnCacheAuthorization), TimeStamp = DateTime.Now });
            db.SaveChanges();
            return base.OnCacheAuthorization(httpContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var db = new ArktinMonitorDataAccess();
            db.DebugLogs.Add(new DebugLog() { Message = nameof(HandleUnauthorizedRequest), TimeStamp = DateTime.Now });
            db.SaveChanges();
            base.HandleUnauthorizedRequest(filterContext);
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //bool disableAuthentication = false;
            //System.Threading.Thread.Sleep(100);
            //#if DEBUG
            //        disableAuthentication = true;
            //#endif

            //            if (disableAuthentication)
            //                return true;
            var db = new ArktinMonitorDataAccess();
            db.DebugLogs.Add(new DebugLog(){Message = httpContext.Request.Headers["Authorization"] ,TimeStamp = DateTime.Now} );
            db.SaveChanges();

            
            return base.AuthorizeCore(httpContext);
        }
    }
}