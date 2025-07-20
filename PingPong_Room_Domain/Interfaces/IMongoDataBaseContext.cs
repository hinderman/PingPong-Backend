using MongoDB.Driver;
using PingPong_Room_Domain.Entities;

namespace PingPong_Room_Domain.Interfaces
{
    public interface IMongoDataBaseContext : IAsyncDisposable
    {
        IMongoCollection<Players> Players { get; }
        IMongoCollection<Rooms> Rooms { get; }
        IMongoCollection<T> GetCollection<T>(string name);

        bool HasActiveTransaction { get; }
        bool IsDisposed { get; }
    }
}
