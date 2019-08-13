using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TestN1.Infrastructure
{
    public class InternalServerErrorResult : ObjectResult
    {
        public InternalServerErrorResult(object error)
            : base(error)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}