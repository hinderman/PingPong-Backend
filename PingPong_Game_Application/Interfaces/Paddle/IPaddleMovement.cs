using PingPong_Game_Domain.ValueObjects;

namespace PingPong_Game_Application.Interfaces.Paddle
{
    public interface IPaddleMovement
    {
        Task HandleMovementAsync(Guid playerId, Position newPosition, string roomId);
    }
}
