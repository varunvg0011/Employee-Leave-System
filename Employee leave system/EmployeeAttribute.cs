using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Web;


namespace Employee_leave_system
{
    public class EmployeeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string sessionRole = context.HttpContext.Session.GetString("EmpRole");
            if (sessionRole != "Employee")
            {

                context.Result =
                new RedirectToRouteResult(new RouteValueDictionary
                         {
                              { "action", "UnauthorizedAccess" },
                            { "controller", "Employee" }
                          });
                return;
            }
            else
            {
                //whatever request is next, it will process and continue 
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
