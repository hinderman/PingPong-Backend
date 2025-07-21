namespace PingPong_Game_Application.Dtos
{
    public class PositionDto
    {
        public int X { get; set; }
        public int Y { get; set; }
        public PositionDto() { }
        public PositionDto(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
