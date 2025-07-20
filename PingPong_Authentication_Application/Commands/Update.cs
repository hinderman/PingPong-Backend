using ErrorOr;
using MediatR;

namespace PingPong_Authentication_Application.Commands
{
    public record Update(Guid Id, string Nickname, string Password) : IRequest<ErrorOr<Unit>> { }
}
