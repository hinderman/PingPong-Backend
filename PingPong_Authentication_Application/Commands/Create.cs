using ErrorOr;
using MediatR;

namespace PingPong_Authentication_Application.Commands
{
    public record Create(string Email, string Nickname, string Password) : IRequest<ErrorOr<Unit>> { }
}
