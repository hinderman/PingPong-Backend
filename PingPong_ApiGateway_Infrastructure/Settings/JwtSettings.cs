namespace PingPong_ApiGateway_Infrastructure.Settings
{
    public class JwtSettings
    {
        public string? Secret { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public int Minutes { get; set; }
    }
}
