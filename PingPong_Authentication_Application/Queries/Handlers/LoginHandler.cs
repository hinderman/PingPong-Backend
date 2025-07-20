using ErrorOr;
using MediatR;
using PingPong_Authentication_Application.Dtos;
using PingPong_Authentication_Domain.Entities;
using PingPong_Authentication_Domain.Repositories;
using PingPong_Authentication_Domain.Services;
using PingPong_Authentication_Domain.ValueObjects;

namespace PingPong_Authentication_Application.Queries.Handlers
{
    internal class LoginHandler(IPassword password, IRepository repository, IJwt jwt) : IRequestHandler<Login, ErrorOr<UsersDto>>
    {
        private readonly IPassword _password = password ?? throw new ArgumentNullException(nameof(password));
        private readonly IRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        private readonly IJwt _jwt = jwt ?? throw new ArgumentNullException(nameof(jwt));

        public async Task<ErrorOr<UsersDto>> Handle(Login request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return Error.Conflict("User.Password", "Se requiere la contraseña.");
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return Error.Conflict("User.Email", "Se requiere el email.");
            }

            if (Email.Create(request.Email) is not Email email)
            {
                return Error.Conflict("User.Email", "Email no valido.");
            }

            if (await _repository.GetByEmail(email) is not Users user)
            {
                return Error.NotFound("User", "Email o contraseña invalida");
            }

            if (await _password.Verify(user.Hash, user.Salt, request.Password))
            {
                return Error.NotFound("User", "Email o contraseña invalida");
            }

            string? token = user.Token;

            if (string.IsNullOrWhiteSpace(token))
            {
                token = await _jwt.Generate(user.Id, user.Email.Value);
                await _repository.SetToken(user.Id, token);
            }

            UsersDto usersDto = new()
            {
                Token = token,
                Id = user.Id,
                Email = user.Email?.Value,
                Nickname = user.Nickname,
                State = user.State
            };

            return usersDto;
        }
    }
}
