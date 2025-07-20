using ErrorOr;
using MediatR;
using PingPong_Room_Application.Dtos;

namespace PingPong_Room_Application.Queries
{
    public record GetAll() : IRequest<ErrorOr<IReadOnlyList<RoomsDto>>> { }
}
