using Microsoft.EntityFrameworkCore;
using PingPong_ApiGateway_Domain.Entities;
using PingPong_ApiGateway_Domain.Repositories;
using PingPong_ApiGateway_Domain.ValueObjects;
using PingPong_ApiGateway_Infrastructure.Persistence.Context;

namespace PingPong_ApiGateway_Infrastructure.Repositories
{
    internal class Repository(DataBaseContext dataBaseContext) : IRepository
    {
        private readonly DataBaseContext _dataBaseContext = dataBaseContext ?? throw new ArgumentNullException(nameof(dataBaseContext));
        public async Task<Users?> GetByEmail(Email email) => await _dataBaseContext.Users.SingleOrDefaultAsync(x => x.Email == email && x.State);
    }
}
