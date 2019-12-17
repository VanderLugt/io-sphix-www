using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;

namespace AspNetCoreWebApplication.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string _someFilterParameter;
        private string controller = "";
        private string action = "";
        private string area = "";
        public Dictionary<string, string> communytyType = new Dictionary<string, string>();
        public CustomAuthorizeAttribute()
        {
            communytyType.Add("1", "academia-and-research");
            communytyType.Add("2", "businesses-and-organizations");
            communytyType.Add("3", "data-tech-and-info-systems");
            communytyType.Add("4", "philanthropy-and-relationships");
            communytyType.Add("5", "philosophy-and-religion");
            communytyType.Add("6", "politics-and-government");
           
            //   _someFilterParameter = someFilterParameter;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            var routeValues = context.RouteData.Values;
            
            controller = (string)routeValues["controller"];
            action = (string)routeValues["action"];
            area = (string)routeValues["area"];
            if (!user.Identity.IsAuthenticated)
            {
               
                // it isn't needed to set unauthorized result 
                // as the base class already requires the user to be authenticated
                // this also makes redirect to a login page work properly
                //context.Result = new UnauthorizedResult();
                var myQueryString = context.HttpContext.Request.Query;

                if (controller== "CommunityGroup")
                {
                    context.Result = new RedirectToActionResult("Index", "Home", new { returnUrl = controller+"/"+ myQueryString["Id"]});
                }
                else if (controller == "CommunityGroups")
                {
                    context.Result = new RedirectToActionResult("Index", "Home", new { returnUrl = controller + "/"+ communytyType[myQueryString["Id"]]});
                }
                //else if (controller == "MoreGroups")
                //{
                //    context.Result = new RedirectToActionResult("Index", "Home", new { returnUrl = controller + "/" + communytyType[myQueryString["Id"]] });
                //}
                else
                {
                    context.Result = new RedirectToActionResult("Index", "Home", new { returnUrl = context.HttpContext.Request.Path });
                }
                return;
            }
            if  (!string.IsNullOrEmpty(area) && area.ToLower() == "superadmin" && !user.IsInRole("admin"))
            {
                context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
                return;
            }
           
           
            // you can also use registered services
            //var someService = context.HttpContext.RequestServices.GetService<IRoleService>();

            //var isAuthorized = someService.IsUserAuthorized(user.Identity.Name, _someFilterParameter);
            //if (!isAuthorized)
            //{
            //    context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
            //    return;
            //}
        }
        private void checkRights()
        {

        }
    }
}
