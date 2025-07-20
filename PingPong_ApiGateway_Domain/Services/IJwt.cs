namespace PingPong_ApiGateway_Domain.Services
{
    public interface IJwt
    {
        Task<string> Generate(Guid id, string email);
    }
}
