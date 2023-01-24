using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Web;


namespace Employee_leave_system
{
    public class AdminAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            string sessionRole = context.HttpContext.Session.GetString("AdminRole");
            if (sessionRole != "Admin")
            {

                context.Result =
                new RedirectToRouteResult(new RouteValueDictionary
                         {
                              { "action", "UnauthorizedAccess" },
                            { "controller", "Admin" }
                          });
                return;
            }
            else
            {
                //whatever request is next, it will process and continue 
                return;
            }
           
        }
    }
}
