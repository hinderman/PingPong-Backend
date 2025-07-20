using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using PingPong_ApiGateway_Api.Settings;
using PingPong_ApiGateway_Application.Settings;
using PingPong_ApiGateway_Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentation().AddInfrastructure(builder.Configuration).AddApplication();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

await app.UseOcelot();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
