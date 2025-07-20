using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using PingPong_Room_Domain.Interfaces;
using PingPong_Room_Domain.Repositories;
using PingPong_Room_Infrastructure.Persistence.Context;
using PingPong_Room_Infrastructure.Repositories;

namespace PingPong_Room_Infrastructure.Settings
{
    public static class SettingsInfrastructure
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var mongoSettings = configuration.GetSection("MongoSettings").Get<MongoSettings>();
            serviceCollection.AddSingleton<IMongoClient>(serviceProvider => new MongoClient(mongoSettings?.ConnectionString));
            serviceCollection.AddScoped<IMongoDataBaseContext>(serviceProvider =>
            {
                var client = serviceProvider.GetRequiredService<IMongoClient>();
                return new MongoDataBaseContext(client, mongoSettings?.DatabaseName!);
            });

            serviceCollection.AddScoped<IUnitOfWork>(x => x.GetRequiredService<MongoDataBaseContext>());
            serviceCollection.AddScoped<IRepository, Repository>();

            return serviceCollection;
        }
    }
}
