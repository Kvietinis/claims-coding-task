using Claims.Auditing.Abstractions;
using Claims.Contracts;
using EnsureThat;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Claims.ActionFilters
{
    public class AuditingActionFilterAttribute : ActionFilterAttribute
    {
        private const string Post = "POST";
        private const string Delete = "DELETE";

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

            if (string.Equals(method, Post, StringComparison.OrdinalIgnoreCase))
            {
                var result = context.Result;

                await AuditPost(result, controllerName);

                return;

            }

            if (string.Equals(method, Delete, StringComparison.OrdinalIgnoreCase))
            {
                var path = context.HttpContext.Request.Path;

                await AuditDelete(path, controllerName);

                return;
            }
        }

        private async Task AuditPost(IActionResult result, string controller)
        {
            var actionResult = result as CreatedResult;

            if (actionResult == null)
            {
                return;
            }

            if (string.Equals(controller, "claims", StringComparison.OrdinalIgnoreCase))
            {
                var claim = actionResult.Value as ClaimDto;
                await _auditer.AuditClaim(claim?.Id, Post);

                return;
            }

            if (string.Equals(controller, "covers", StringComparison.OrdinalIgnoreCase))
            {
                var cover = actionResult.Value as CoverDto;
                await _auditer.AuditCover(cover?.Id, Post);

                return;
            }
        }

        private async Task AuditDelete(PathString path, string controller)
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

            if (string.Equals(controller, "claims", StringComparison.OrdinalIgnoreCase))
            {
                await _auditer.AuditClaim(id, Delete);

                return;
            }

            if (string.Equals(controller, "covers", StringComparison.OrdinalIgnoreCase))
            {
                await _auditer.AuditCover(id, Delete);

                return;
            }
        }
    }
}
