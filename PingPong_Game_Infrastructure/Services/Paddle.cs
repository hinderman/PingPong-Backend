using PingPong_Game_Domain.Interfaces;
using PingPong_Game_Domain.ValueObjects;

namespace PingPong_Game_Infrastructure.Services
{
    public class Paddle : IPaddle
    {
        public void MovePaddle(PingPong_Game_Domain.Entities.Paddle paddle, Position newPosition)
        {
            paddle.MoveTo(newPosition);
        }
    }
}
