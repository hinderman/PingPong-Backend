using PingPong_Game_Application.Interfaces;
using PingPong_Game_Application.Interfaces.Paddle;
using PingPong_Game_Domain.Interfaces;
using PingPong_Game_Domain.ValueObjects;

namespace PingPong_Game_Application.Paddle
{
    public class PaddleMovement(IPaddle paddleService, IPaddleMoveOutput outputPort) : IPaddleMovement
    {
        private readonly IPaddle _paddleService = paddleService ?? throw new ArgumentNullException(nameof(paddleService));
        private readonly IPaddleMoveOutput _outputPort = outputPort ?? throw new ArgumentNullException(nameof(outputPort));

        public async Task HandleMovementAsync(Guid playerId, Position newPosition, string roomId)
        {
            var paddle = new PingPong_Game_Domain.Entities.Paddle(playerId, newPosition);
            _paddleService.MovePaddle(paddle, newPosition);

            await _outputPort.BroadcastPaddleMoved(roomId, playerId, newPosition);
        }
    }
}
