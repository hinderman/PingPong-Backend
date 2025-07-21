using PingPong_Game_Domain.ValueObjects;

namespace PingPong_Game_Application.Interfaces
{
    public interface IPaddleMoveOutput
    {
        Task BroadcastPaddleMoved(string roomId, Guid playerId, Position newPosition);
    }
}
