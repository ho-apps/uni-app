using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestN1.Middleware
{
    public class DataMiddleware
    {
        private readonly RequestDelegate _next;

        public DataMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method != HttpMethods.Get)
            {
                context.Response.StatusCode = 404;
            }

            var get = context.Request.Query["get"];
            
            var insert = context.Request.Query["insert"];

            if (string.IsNullOrWhiteSpace(get) || string.IsNullOrWhiteSpace(insert))
            {
                context.Response.StatusCode = 404;
            }
            else
            {            
                await _next.Invoke(context);
            }
        }
    }
}
