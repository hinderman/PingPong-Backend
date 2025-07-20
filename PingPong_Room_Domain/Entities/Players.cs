using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PingPong_Room_Domain.ValueObjects;

namespace PingPong_Room_Domain.Entities
{
    public class Players
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; private set; }

        [BsonElement("email")]
        [BsonRequired]
        public Email Email { get; private set; }

        private Players() { }

        public Players(Guid id, Email email)
        {
            Id = id;
            Email = email ?? throw new ArgumentNullException(nameof(email));
        }
    }
}
