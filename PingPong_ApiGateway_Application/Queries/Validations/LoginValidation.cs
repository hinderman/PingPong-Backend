using FluentValidation;

namespace PingPong_ApiGateway_Application.Queries.Validations
{
    internal class LoginValidation : AbstractValidator<Login>
    {
        public LoginValidation()
        {
            RuleFor(rule => rule.Email).NotEmpty().WithName("Email").WithMessage("El {PropertyName} es requerido")
                .EmailAddress().WithName("Email").WithMessage("El {PropertyName} no posee el formato requerido")
                .MaximumLength(50).WithName("Email").WithMessage("El {PropertyName} debe tener a lo mucho 50 caracteres");

            RuleFor(rule => rule.Password).NotEmpty().WithMessage("Se requiere contraseña.")
                .NotNull().WithMessage("Se requiere contraseña.");
        }
    }
}
