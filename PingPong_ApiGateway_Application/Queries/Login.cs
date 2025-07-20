using ErrorOr;
using MediatR;
using PingPong_ApiGateway_Application.Dtos;

namespace PingPong_ApiGateway_Application.Queries
{
    public record Login(string Email, string Password) : IRequest<ErrorOr<UsersDto>> { }
}
