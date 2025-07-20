using Microsoft.EntityFrameworkCore;
using PingPong_ApiGateway_Domain.Entities;

namespace PingPong_ApiGateway_Domain.Interfaces
{
    public interface IDataBaseContext
    {
        DbSet<Users> Users { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
