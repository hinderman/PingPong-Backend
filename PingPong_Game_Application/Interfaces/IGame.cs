namespace PingPong_Game_Application.Interfaces
{
    public interface IGame
    {
        Task NotifyPlayerJoined(Guid roomId);
    }
}
