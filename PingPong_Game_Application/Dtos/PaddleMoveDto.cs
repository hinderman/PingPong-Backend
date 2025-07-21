namespace PingPong_Game_Application.Dtos
{
    public class PaddleMoveDto
    {
        public Guid PlayerId { get; set; }
        public PositionDto NewPosition { get; set; }
        public string RoomId { get; set; }
    }
}
