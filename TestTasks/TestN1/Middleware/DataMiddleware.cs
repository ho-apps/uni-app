using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using TestTasks.Data.Domains;
using TestTasks.Data.Domains.Models;

namespace TestN1.Middleware
{
    public class DataMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly EntityDataProvider _dataProvider;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="next"></param> 
        /// <param name="dataProvider">провайдер обработки данных</param>
        public DataMiddleware(RequestDelegate next, EntityDataProvider dataProvider)
        {
            _next = next;
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method != HttpMethods.Get)
            {
                context.Response.StatusCode = 404;
            }

            const string get = "get";
            const string insert = "insert";

            StringValues getValue = context.Request.Query[get];
            
            StringValues insertValue = context.Request.Query[insert];

            if (!string.IsNullOrWhiteSpace(getValue))
            {
                if (context.Request.QueryString.HasValue)
                {
                    context.Request.Query.TryGetValue(get, out getValue);
                    //id = id.TrimStart(@"?get=");
                    if (Guid.TryParse(getValue.ToString(), out Guid guid))
                    {
                     Entity res = await _dataProvider.FindByIdAsync(guid);

                     return res;
                    }
                    else
                    {
                        throw new Exception($"Получен некорректный Id.");
                    }
                    //_dataProvider.FindByIdAsync();
                    await _next.Invoke(context);
                }
                else
                {
                    throw new Exception($"Для запроса должен быть заполнен обязательный параметр get.");
                }
                
            }
            else if (!string.IsNullOrWhiteSpace(insertValue))
            {
                await _next.Invoke(context);
            }
            else
            {
                context.Response.StatusCode = 404;
            }
        }
    }
}
