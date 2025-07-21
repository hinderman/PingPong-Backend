using System.Text.Json;
using PingPong_Game_Application.Dtos;
using PingPong_Game_Application.Interfaces.Incoming;

namespace PingPong_Game_Infrastructure.Services
{
    public class SocketConnectionHandler(IPaddleMoveInput paddleInputPort)
    {
        private readonly IPaddleMoveInput _paddleInputPort = paddleInputPort;

        public async Task HandleSocketMessage(string messageJson)
        {
            var message = JsonSerializer.Deserialize<PaddleMoveDto>(messageJson);

            await _paddleInputPort.OnPaddleMoveReceived(message.PlayerId, message?.NewPosition, message?.RoomId);
        }
    }
}