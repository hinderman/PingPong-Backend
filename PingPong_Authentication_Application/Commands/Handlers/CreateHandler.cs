using ErrorOr;
using MediatR;
using PingPong_Authentication_Domain.Entities;
using PingPong_Authentication_Domain.Repositories;
using PingPong_Authentication_Domain.Services;
using PingPong_Authentication_Domain.ValueObjects;

namespace PingPong_Authentication_Application.Commands.Handlers
{
    internal class CreateHandler(IPassword password, IRepository repository) : IRequestHandler<Create, ErrorOr<Unit>>
    {
        private readonly IPassword _password = password ?? throw new ArgumentNullException(nameof(password));
        private readonly IRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

        public async Task<ErrorOr<Unit>> Handle(Create request, CancellationToken cancellationToken)
        {
            if (Email.Create(request.Email) is not Email email)
            {
                return Error.Conflict("User.Email", "Email no valido.");
            }

            if (await _repository.ExistsEmail(email))
            {
                return Error.Conflict("User.Email", "El Email ya existe.");
            }

            if (await _repository.ExistsNickname(request.Nickname))
            {
                return Error.Conflict("User.Nickname", "El Nickname ya existe.");
            }

            byte[] salt = await _password.Salt();
            byte[] passwordHash = await _password.Hash(request.Password, salt);

            Users user = new(Guid.NewGuid(), email, request.Nickname, passwordHash, salt);

            await _repository.Create(user);

            return Unit.Value;
        }
    }
}
