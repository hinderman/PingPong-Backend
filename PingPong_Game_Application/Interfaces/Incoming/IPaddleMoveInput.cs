using PingPong_Game_Application.Dtos;
using PingPong_Game_Domain.ValueObjects;

namespace PingPong_Game_Application.Interfaces.Incoming
{
    public interface IPaddleMoveInput
    {
        Task OnPaddleMoveReceived(Guid playerId, PositionDto newPosition, string roomId);
    }
}
