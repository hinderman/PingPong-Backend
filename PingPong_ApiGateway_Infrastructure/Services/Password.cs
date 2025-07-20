using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;
using PingPong_ApiGateway_Domain.Services;

namespace PingPong_ApiGateway_Infrastructure.Services
{
    public class Password : IPassword
    {
        public Task<byte[]> Hash(string password, byte[] salt)
        {
            using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));
            argon2.Salt = salt;
            argon2.DegreeOfParallelism = 4;
            argon2.MemorySize = 65536;
            argon2.Iterations = 4;

            return Task.FromResult(argon2.GetBytes(32));
        }

        public async Task<bool> Verify(byte[] hash, byte[] salt, string password)
        {
            byte[] newHash = await Hash(password, salt);
            return newHash.SequenceEqual(hash);
        }

        public Task<byte[]> Salt()
        {
            return Task.FromResult(RandomNumberGenerator.GetBytes(16));
        }
    }
}
