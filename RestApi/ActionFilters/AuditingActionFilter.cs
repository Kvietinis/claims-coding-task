using Claims.Auditing.Abstractions;
using EnsureThat;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Claims.ActionFilters
{
    public class AuditingActionFilter : IAsyncActionFilter
    {
        private readonly IAuditer _auditer;

        public AuditingActionFilter(IAuditer auditer)
        {
            Ensure.That(auditer, nameof(auditer)).IsNotNull();

            _auditer = auditer;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            //ClaimDto claim;
            //CoverDto cover;

            //if (method.Equals("post", StringComparison.OrdinalIgnoreCase))
            //{
            //    var result = context.Result as CreatedResult;
            //    var value = result?.Value;

            //    claim = value as ClaimDto;
            //    cover = value as CoverDto;
            //}

            //result.Con

            await next();

            var id = GetId(method);
            _auditer.AuditClaim(id, method);
        }

        private static string GetId(string method)
        {
            var result = default(string);

            if (method.Equals("post", StringComparison.OrdinalIgnoreCase))
            {
                return result;
            }

            if (method.Equals("delete", StringComparison.OrdinalIgnoreCase))
            {
                return result;
            }

            return result;
        }
    }
}
