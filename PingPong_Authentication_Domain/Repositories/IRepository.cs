using PingPong_Authentication_Domain.Entities;
using PingPong_Authentication_Domain.ValueObjects;

namespace PingPong_Authentication_Domain.Repositories
{
    public interface IRepository
    {
        Task Create(Users users);
        Task SetToken(Guid id, string token);
        Task Delete(Guid id);
        Task<Users?> GetByEmail(Email email);
        Task<Users?> GetById(Guid id);
        Task Update(Users users);
        Task<bool> ExistsEmail(Email email);
        Task<bool> ExistsNickname(string nickname);
        Task<bool> ExistsId(Guid id);
    }
}
