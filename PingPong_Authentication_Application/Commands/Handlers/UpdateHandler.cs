using ErrorOr;
using MediatR;
using PingPong_Authentication_Domain.Entities;
using PingPong_Authentication_Domain.Repositories;
using PingPong_Authentication_Domain.Services;

namespace PingPong_Authentication_Application.Commands.Handlers
{
    internal class UpdateHandler(IPassword password, IRepository repository) : IRequestHandler<Update, ErrorOr<Unit>>
    {
        private readonly IPassword _password = password ?? throw new ArgumentNullException(nameof(password));
        private readonly IRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

        public async Task<ErrorOr<Unit>> Handle(Update request, CancellationToken cancellationToken)
        {
            if (await _repository.ExistsNickname(request.Nickname))
            {
                return Error.Conflict("User.Nickname", "El Nickname ya existe.");
            }

            if (await _repository.GetById(request.Id) is not Users user)
            {
                return Error.NotFound("User.NotFound", "El usuario que desea actualizar, no fue encontrado.");
            }

            byte[] salt = await _password.Salt();
            byte[] passwordHash = await _password.Hash(request.Password, salt);

            Users userUpdate = new(user.Id, user.Email, request.Nickname, passwordHash, salt);

            await _repository.Update(userUpdate);

            return Unit.Value;
        }
    }
}
