using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEDESOL.UI.Controllers
{
    public class SessionExpireFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;

            if (ctx.Session == null || ctx.Session["userData"] == null)
            {
                filterContext.Result = new RedirectResult("~/Login/Login");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}