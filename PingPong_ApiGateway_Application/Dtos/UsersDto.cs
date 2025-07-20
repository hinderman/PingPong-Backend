namespace PingPong_ApiGateway_Application.Dtos
{
    public class UsersDto
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? Nickname { get; set; }
        public string? Token { get; set; }
        public bool State { get; set; }
    }
}
