using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using PingPong_Room_Domain.Enumeration;

namespace PingPong_Room_Domain.Entities
{
    public class Rooms
    {
        [BsonId]
        [BsonElement("id")]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; private set; } = Guid.NewGuid();

        [BsonElement("players")]
        [BsonRequired]
        public IReadOnlyList<Players> Players { get; private set; } = [];

        [BsonElement("status")]
        [BsonRequired]
        public Status Status { get; private set; }

        private Rooms() { }

        public Rooms(Guid id, IReadOnlyList<Players> players, Status status)
        {
            Id = id;
            Players = players ?? throw new ArgumentNullException(nameof(players));
            Status = status;
        }
    }
}
