using PingPong_ApiGateway_Domain.Entities;
using PingPong_ApiGateway_Domain.ValueObjects;

namespace PingPong_ApiGateway_Domain.Repositories
{
    public interface IRepository
    {
        Task<Users?> GetByEmail(Email email);
    }
}
