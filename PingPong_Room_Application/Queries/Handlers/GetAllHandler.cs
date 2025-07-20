using ErrorOr;
using MediatR;
using PingPong_Room_Application.Dtos;
using PingPong_Room_Domain.Entities;
using PingPong_Room_Domain.Repositories;

namespace PingPong_Room_Application.Queries.Handlers
{
    internal class GetAllHandler(IRepository repository) : IRequestHandler<GetAll, ErrorOr<IReadOnlyList<RoomsDto>>>
    {
        private readonly IRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        public async Task<ErrorOr<IReadOnlyList<RoomsDto>>> Handle(GetAll request, CancellationToken cancellationToken)
        {
            IReadOnlyList<Rooms> lstRooms = await _repository.GetAll();

            return lstRooms.Select(r => new RoomsDto() 
            { 
                Id = r.Id, 
                Players = r.Players.Select(p => new PlayersDto() { Id = p.Id, Email = p.Email.Value }).ToList(), 
                Status = r.Status.ToString() 
            }).ToList();
        }
    }
}
