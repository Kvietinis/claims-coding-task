using Claims.Auditing.Abstractions;
using Claims.Contracts;
using EnsureThat;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Claims.ActionFilters
{
    public class AuditingActionFilterAttribute : ActionFilterAttribute
    {
        private readonly IAuditer _auditer;

        public AuditingActionFilterAttribute(IAuditer auditer)
        {
            Ensure.That(auditer, nameof(auditer)).IsNotNull();

            _auditer = auditer;
        }

        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            var controller = context.Controller as ControllerBase;

            var controllerName = controller?.ControllerContext.ActionDescriptor.ControllerName;

            await next();

            if (string.Equals(method, "post", StringComparison.OrdinalIgnoreCase))
            {
                var result = context.Result;

                AuditPost(result, controllerName);

            }

            if (string.Equals(method, "delete", StringComparison.OrdinalIgnoreCase))
            {
                var path = context.HttpContext.Request.Path;

                AuditDelete(path, controllerName);
            }
        }

        private void AuditPost(IActionResult result, string controller)
        {
            var actionResult = result as CreatedResult;

            if (actionResult == null)
            {
                return;
            }

            const string method = "POST";

            if (string.Equals(controller, "claims", StringComparison.OrdinalIgnoreCase))
            {
                var claim = actionResult.Value as ClaimDto;
                _auditer.AuditClaim(claim?.Id, method);

                return;
            }

            if (string.Equals(controller, "covers", StringComparison.OrdinalIgnoreCase))
            {
                var cover = actionResult.Value as CoverDto;
                _auditer.AuditCover(cover?.Id, method);

                return;
            }
        }

        private void AuditDelete(PathString path, string controller)
        {
            if (!path.HasValue)
            {
                return;
            }

            var id = path.Value?.Split("/").LastOrDefault();

            if (id == null)
            {
                return;
            }

            const string method = "DELETE";

            if (string.Equals(controller, "claims", StringComparison.OrdinalIgnoreCase))
            {
                _auditer.AuditClaim(id, method);

                return;
            }

            if (string.Equals(controller, "covers", StringComparison.OrdinalIgnoreCase))
            {
                _auditer.AuditCover(id, method);

                return;
            }
        }
    }
}
