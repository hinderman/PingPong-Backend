using Microsoft.EntityFrameworkCore;
using PingPong_Authentication_Domain.Entities;

namespace PingPong_Authentication_Domain.Interfaces
{
    public interface IDataBaseContext
    {
        DbSet<Users> Users { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
