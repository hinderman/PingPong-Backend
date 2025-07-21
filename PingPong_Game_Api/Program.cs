using PingPong_Game_Api.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapHub<GameHub>("/gamehub");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
