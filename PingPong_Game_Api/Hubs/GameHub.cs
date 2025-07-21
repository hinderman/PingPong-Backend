using Microsoft.AspNetCore.SignalR;
using PingPong_Game_Application.Dtos;
using PingPong_Game_Application.Interfaces.Incoming;

namespace PingPong_Game_Api.Hubs
{
    public class GameHub(IPaddleMoveInput paddleMoveInput) : Hub
    {
        private readonly IPaddleMoveInput _dispatcher = paddleMoveInput ?? throw new ArgumentNullException(nameof(paddleMoveInput));
        public async Task MovePaddle(string roomId, string playerId, int x, int y)
        {
            await _dispatcher.OnPaddleMoveReceived(Guid.Parse(playerId), new PositionDto(x, y), roomId);
        }

        public async Task SendToOthers(string roomId, object data)
        {
            await Clients.OthersInGroup(roomId).SendAsync("PaddleMoved", data);
        }
    }
}