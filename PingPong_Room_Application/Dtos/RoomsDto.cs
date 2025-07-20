namespace PingPong_Room_Application.Dtos
{
    public class RoomsDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public IReadOnlyList<PlayersDto> Players { get; set; } = [];
        public string? Status { get; set; }
    }
}
