using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PingPong_Room_Api.Common
{
    [ApiController]
    public abstract class ApiBaseController : ControllerBase
    {
        private ISender? _ISender;

        protected ISender Sender => _ISender ??= HttpContext.RequestServices.GetRequiredService<ISender>();

        protected IActionResult Problem(List<Error> pErrors)
        {
            if (pErrors.Count is 0)
            {
                return Problem();
            }

            if (pErrors.All(error => error.Type == ErrorType.Validation))
            {
                return ValidationProblem(pErrors);
            }

            HttpContext.Items[Constants.Error] = pErrors;

            return Problem(pErrors[0]);
        }

        private IActionResult Problem(Error pError)
        {
            var statusCode = pError.Type switch
            {
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            return Problem(statusCode: statusCode, title: pError.Description);
        }

        private IActionResult ValidationProblem(List<Error> errors)
        {
            var modelStateDictionary = new ModelStateDictionary();

            foreach (var error in errors)
            {
                modelStateDictionary.AddModelError(error.Code, error.Description);
            }

            return ValidationProblem(modelStateDictionary);
        }
    }
}
