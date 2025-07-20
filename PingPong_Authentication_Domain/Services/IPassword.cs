namespace PingPong_Authentication_Domain.Services
{
    public interface IPassword
    {
        Task<bool> Verify(byte[] hash, byte[] salt, string password);
        Task<byte[]> Hash(string password, byte[] salt);
        Task<byte[]> Salt();
    }
}
