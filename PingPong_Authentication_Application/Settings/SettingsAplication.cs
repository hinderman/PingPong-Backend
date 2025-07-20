using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PingPong_Authentication_Application.Settings.Behaviors;

namespace PingPong_Authentication_Application.Settings
{
    public static class SettingsAplication
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssemblyContaining<AssemblyReference>();
            });

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationsBehaviors<,>));

            services.AddValidatorsFromAssemblyContaining<AssemblyReference>();

            return services;
        }
    }
}
