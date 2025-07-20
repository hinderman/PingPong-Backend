using ErrorOr;
using MediatR;

namespace PingPong_Authentication_Application.Commands
{
    public record Delete(Guid Id) : IRequest<ErrorOr<Unit>> { }
}
