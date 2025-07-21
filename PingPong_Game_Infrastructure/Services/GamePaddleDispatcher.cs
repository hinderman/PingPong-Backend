using PingPong_Game_Application.Interfaces.Incoming;
using PingPong_Game_Application.Interfaces.Paddle;
using PingPong_Game_Domain.ValueObjects;

namespace PingPong_Game_Infrastructure.Services
{
    internal class GamePaddleDispatcher(IPaddleMovement paddleMovement) : IPaddleMoveInput
    {
        private readonly IPaddleMovement _paddleMovement = paddleMovement ?? throw new ArgumentNullException(nameof(paddleMovement));
        public async Task OnPaddleMoveReceived(Guid playerId, Position newPosition, string roomId)
        {
            await _paddleMovement.HandleMovementAsync(playerId, newPosition, roomId);
        }
    }
}
