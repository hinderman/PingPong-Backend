using PingPong_Room_Domain.Entities;

namespace PingPong_Room_Domain.Repositories
{
    public interface IRepository
    {
        Task<Rooms> Create(Players players);
        Task<Rooms> Update(Rooms rooms);
        Task<Rooms?> GetById(Guid id);
        Task<IReadOnlyList<Rooms>> GetAll();
        Task Delete(Guid id);
    }
}
