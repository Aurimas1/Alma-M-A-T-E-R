using Microsoft.AspNetCore.Http;
using Serilog;
using System.Threading.Tasks;

namespace API.Middlewares
{
    public class LogingMiddleware : IMiddleware
    {
        static readonly ILogger Log = Serilog.Log.ForContext<LogingMiddleware>();

        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            return next(context);
        }

        private string GenerateMessage(string user, string role, string time, string @class, string method)
        {
            return $"{user} {role} {time} {@class} {method}";
        }
    }
}
