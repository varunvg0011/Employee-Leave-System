using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Web;

namespace Employee_leave_system
{
    public class CustomAttributeDemo: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string sessionVarUsername = context.HttpContext.Session.GetString("Username");
            if (sessionVarUsername==null)
            {

                context.Result =
                new RedirectToRouteResult(new RouteValueDictionary
                         {
                              { "action", "AdminLogin" },
                            { "controller", "Admin" }
                          });
                return;
            }
            else if (sessionVarUsername!=null || sessionVarUsername.Trim().Length != 0)
            {
                //whatever request is next, it will process and continue 
                return;
            }
            
            base.OnActionExecuting(context);
        }


    }
}
