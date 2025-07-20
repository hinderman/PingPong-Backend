using ErrorOr;
using FluentValidation;
using MediatR;

namespace PingPong_ApiGateway_Application.Settings.Behaviors
{
    public class ValidationsBehaviors<IRequest, IResponse>(IValidator<IRequest>? pIValidator = null) : IPipelineBehavior<IRequest, IResponse> where IRequest : IRequest<IResponse> where IResponse : IErrorOr
    {
        private readonly IValidator<IRequest>? _IValidator = pIValidator;
        public async Task<IResponse> Handle(IRequest request, RequestHandlerDelegate<IResponse> next, CancellationToken cancellationToken)
        {
            if (_IValidator is null)
            {
                return await next();
            }

            var validationsResult = await _IValidator.ValidateAsync(request, cancellationToken);

            if (validationsResult.IsValid)
            {
                return await next();
            }

            IEnumerable<Error> errors = validationsResult.Errors.ConvertAll(validations => Error.Validation(validations.PropertyName, validations.ErrorMessage));

            return (dynamic)errors;
        }
    }
}
