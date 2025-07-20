using MongoDB.Driver;
using PingPong_Room_Domain.Entities;
using PingPong_Room_Domain.Enumeration;
using PingPong_Room_Domain.Interfaces;
using PingPong_Room_Domain.Repositories;

namespace PingPong_Room_Infrastructure.Repositories
{
    internal class Repository(IMongoDataBaseContext database) : IRepository
    {
        private readonly IMongoCollection<Rooms> _collection = database.Rooms;

        public async Task<Rooms> Create(Players players)
        {
            Rooms rooms = new(Guid.NewGuid(), [players], Status.Waiting);
            await _collection.InsertOneAsync(rooms);

            return rooms;
        }

        public async Task Delete(Guid id)
        {
            var filter = Builders<Rooms>.Filter.Eq("Id", id);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<IReadOnlyList<Rooms>> GetAll()
        {
            var filter = Builders<Rooms>.Filter.Eq("status", Status.Waiting);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<Rooms?> GetById(Guid id)
        {
            var filter = Builders<Rooms>.Filter.Eq("Id", id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Rooms> Update(Rooms rooms)
        {
            var filter = Builders<Rooms>.Filter.Eq("Id", rooms.Id);
            await _collection.ReplaceOneAsync(filter, rooms);

            return rooms;
        }
    }
}
