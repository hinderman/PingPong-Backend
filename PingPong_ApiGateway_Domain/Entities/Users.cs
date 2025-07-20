using PingPong_ApiGateway_Domain.ValueObjects;

namespace PingPong_ApiGateway_Domain.Entities
{
    public class Users
    {
        public Guid Id { get; private set; }
        public Email Email { get; private set; }
        public string Nickname { get; private set; }
        public byte[] Hash { get; private set; }
        public byte[] Salt { get; private set; }
        public bool State { get; private set; }

        private Users() { }

        public Users(Guid id, Email email, string nickname, byte[] hash, byte[] salt, bool state = true)
        {
            Id = id;
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Hash = hash ?? throw new ArgumentNullException(nameof(hash));
            Salt = salt ?? throw new ArgumentNullException(nameof(salt));
            State = state;
            Nickname = nickname ?? throw new ArgumentNullException(nameof(nickname));
        }
    }
}
