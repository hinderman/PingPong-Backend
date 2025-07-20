using System.Text;
using ErrorOr;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PingPong_Authentication_Domain.Interfaces;
using PingPong_Authentication_Domain.Repositories;
using PingPong_Authentication_Domain.Services;
using PingPong_Authentication_Infrastructure.Persistence.Context;
using PingPong_Authentication_Infrastructure.Repositories;
using PingPong_Authentication_Infrastructure.Services;

namespace PingPong_Authentication_Infrastructure.Settings
{
    public static class SettingsInfrastructure
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            serviceCollection.AddDbContext<DataBaseContext>(options => options.UseNpgsql(configuration.GetConnectionString("DataBaseConnection")));

            serviceCollection.AddScoped<IDataBaseContext>(x => x.GetRequiredService<DataBaseContext>());
            serviceCollection.AddScoped<IUnitOfWork>(x => x.GetRequiredService<DataBaseContext>());
            serviceCollection.AddScoped<IRepository, Repository>();
            serviceCollection.AddScoped<IPassword, Password>();
            serviceCollection.AddScoped<IJwt, Jwt>();

            serviceCollection.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            serviceCollection.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"]!)),
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["JwtSettings:Audience"],
                    ValidateLifetime = true
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (!context.Response.HasStarted)
                        {
                            context.Response.StatusCode = 500;
                            var result = JsonConvert.SerializeObject(Error.Failure("Authentication.Failed", "Ocurrio un error en la autenticacion"));
                            return context.Response.WriteAsync(result);
                        }
                        return Task.CompletedTask;
                    },

                    OnChallenge = context =>
                    {
                        context.HandleResponse();

                        if (!context.Response.HasStarted)
                        {
                            context.Response.StatusCode = 401;
                            var result = JsonConvert.SerializeObject(Error.Unauthorized("Authentication.Unauthorized", "No se encuentra autorizado para acceder a este recurso"));
                            return context.Response.WriteAsync(result);
                        }

                        return Task.CompletedTask;
                    },

                    OnForbidden = context =>
                    {
                        if (!context.Response.HasStarted)
                        {
                            context.Response.StatusCode = 403;
                            var result = JsonConvert.SerializeObject(Error.Forbidden("Authentication.Forbidden", "No posee los permisos necesarios para este recurso"));
                            return context.Response.WriteAsync(result);
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            return serviceCollection;
        }
    }
}
