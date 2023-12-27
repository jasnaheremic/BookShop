using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BookShop.MyHelpers
{
    public class RequiredAuthAttribute : Attribute, IPageFilter
    {
        public string Required { get; set; } = "";
        public string RequiredRole { get; set; } = "";

        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            
        }

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            string role = context.HttpContext.Session.GetString("role");

            if (role == null)
            {
                context.Result = new RedirectResult("/");
            }
            else
            {
                if (RequiredRole.Length > 0 && !RequiredRole.Equals(role))
                {

                    context.Result = new RedirectResult("/");
                }
            }
        }

        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
            
        }
    }
}
