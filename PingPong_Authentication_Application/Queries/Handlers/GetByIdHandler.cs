using ErrorOr;
using MediatR;
using PingPong_Authentication_Application.Dtos;
using PingPong_Authentication_Domain.Entities;
using PingPong_Authentication_Domain.Repositories;

namespace PingPong_Authentication_Application.Queries.Handlers
{
    internal class GetByIdHandler(IRepository repository) : IRequestHandler<GetById, ErrorOr<UsersDto>>
    {
        private readonly IRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        public async Task<ErrorOr<UsersDto>> Handle(GetById request, CancellationToken cancellationToken)
        {
            if (await _repository.GetById(request.Id) is not Users user)
            {
                return Error.NotFound("User.NotFound", "El usuario no fue encontrado.");
            }

            UsersDto userDto = new()
            {
                Token = user.Token,
                Id = user.Id,
                Email = user.Email?.Value,
                Nickname = user.Nickname,
                State = user.State
            };

            return userDto;

            throw new NotImplementedException();
        }
    }
}
