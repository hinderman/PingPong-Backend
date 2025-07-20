namespace PingPong_Authentication_Domain.Services
{
    public interface IJwt
    {
        Task<string> Generate(Guid id, string email);
    }
}
