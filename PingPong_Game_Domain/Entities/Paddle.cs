using PingPong_Game_Domain.ValueObjects;

namespace PingPong_Game_Domain.Entities
{
    public class Paddle
    {
        public Guid PlayerId { get; private set; }
        public Position Position { get; private set; }

        public Paddle(Guid playerId, Position initialPosition)
        {
            PlayerId = playerId;
            Position = initialPosition;
        }

        public void MoveTo(Position newPosition)
        {
            if (newPosition.Y < 0 || newPosition.Y > 100)
            {
                throw new InvalidOperationException("Invalid position");
            }

            Position = newPosition;
        }
    }
}
