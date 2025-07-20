using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ErrorOr;

namespace PingPong_Room_Api.Common
{
    public class ApiProblemDetailsFactory : ProblemDetailsFactory
    {
        private readonly ApiBehaviorOptions _ApiBehaviorOptions;

        public ApiProblemDetailsFactory(ApiBehaviorOptions pApiBehaviorOptions)
        {
            _ApiBehaviorOptions = pApiBehaviorOptions ?? throw new ArgumentNullException(nameof(pApiBehaviorOptions));
        }

        public override ProblemDetails CreateProblemDetails(HttpContext pHttpContext, int? statusCode = null, string? title = null, string? type = null, string? detail = null, string? instance = null)
        {
            statusCode ??= 500;

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Type = type,
                Detail = detail,
                Instance = instance
            };

            ApplyProblemDetailsDefault(pHttpContext, problemDetails, statusCode.Value);

            return problemDetails;
        }

        public override ValidationProblemDetails CreateValidationProblemDetails(HttpContext httpContext, ModelStateDictionary modelStateDictionary, int? statusCode = null, string? title = null, string? type = null, string? detail = null, string? instance = null)
        {
            statusCode ??= 400;

            var problemDetails = new ValidationProblemDetails(modelStateDictionary)
            {
                Status = statusCode,
                Type = type,
                Detail = detail,
                Instance = instance
            };

            if (title != null)
            {
                problemDetails.Title = title;
            }

            ApplyProblemDetailsDefault(httpContext, problemDetails, statusCode.Value);

            return problemDetails;
        }

        private void ApplyProblemDetailsDefault(HttpContext pHttpContext, ProblemDetails pProblemDetails, int pStatusCode)
        {
            pProblemDetails.Status ??= pStatusCode;

            if (_ApiBehaviorOptions.ClientErrorMapping.TryGetValue(pStatusCode, out var apiErrorData))
            {
                pProblemDetails.Title ??= apiErrorData.Title;
                pProblemDetails.Type ??= apiErrorData.Link;
            }

            var traceId = Activity.Current?.Id ?? pHttpContext.TraceIdentifier;

            if (traceId != null)
            {
                pProblemDetails.Extensions["traceId"] = traceId;
            }

            var errors = pHttpContext?.Items[Constants.Error] as List<Error>;

            if (errors is not null)
            {
                pProblemDetails.Extensions.Add("errorCodes", errors.Select(e => e.Code));
            }
        }
    }
}