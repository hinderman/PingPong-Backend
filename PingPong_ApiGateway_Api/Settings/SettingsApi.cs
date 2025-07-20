using PingPong_ApiGateway_Api.Middleware;

namespace PingPong_ApiGateway_Api.Settings
{
    public static class SettingsApi
    {
        public static IServiceCollection AddPresentation(this IServiceCollection pIServiceCollection)
        {
            pIServiceCollection.AddControllers();
            pIServiceCollection.AddEndpointsApiExplorer();
            pIServiceCollection.AddTransient<ApiMiddleware>();

            return pIServiceCollection;
        }
    }
}