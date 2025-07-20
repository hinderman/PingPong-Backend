using ErrorOr;
using MediatR;
using PingPong_Authentication_Application.Dtos;

namespace PingPong_Authentication_Application.Queries
{
    public record GetById(Guid Id) : IRequest<ErrorOr<UsersDto>> { }
}
