using PingPong_Game_Domain.Entities;
using PingPong_Game_Domain.ValueObjects;

namespace PingPong_Game_Domain.Interfaces
{
    public interface IPaddle
    {
        void MovePaddle(Paddle paddle, Position newPosition);
    }
}
