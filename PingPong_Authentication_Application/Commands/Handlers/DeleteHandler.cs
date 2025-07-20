using ErrorOr;
using MediatR;
using PingPong_Authentication_Domain.Entities;
using PingPong_Authentication_Domain.Repositories;

namespace PingPong_Authentication_Application.Commands.Handlers
{
    internal class DeleteHandler(IRepository repository) : IRequestHandler<Delete, ErrorOr<Unit>>
    {
        private readonly IRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        public async Task<ErrorOr<Unit>> Handle(Delete request, CancellationToken cancellationToken)
        {
            if (await _repository.GetById(request.Id) is not Users user)
            {
                return Error.NotFound("User.NotFound", "El usuario que desea actualizar, no fue encontrado.");
            }

            await _repository.Delete(request.Id);
            return Unit.Value;
        }
    }
}
