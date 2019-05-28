using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using API.Extensions;
using System;

namespace API.ActionFilter
{
    public class LogActionFilter : IActionFilter
    {
        private static readonly ILogger logger = Serilog.Log.ForContext<LogActionFilter>();
        private readonly IHttpContextAccessor accessor;

        public LogActionFilter(IHttpContextAccessor accessor)
        {
            //this.logger = logger;
            this.accessor = accessor;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            string controllerName = controllerActionDescriptor?.ControllerName;
            string actionName = controllerActionDescriptor?.ActionName;

            logger.Information($"{accessor.HttpContext.User.Identity.Name}, {accessor.HttpContext.User.GetRole()}, {DateTime.Now}, {controllerName}, {actionName}");
        }
    }
}
