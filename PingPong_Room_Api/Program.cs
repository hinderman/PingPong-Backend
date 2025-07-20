using PingPong_Room_Api.Middleware;
using PingPong_Room_Api.Settings;
using PingPong_Room_Infrastructure.Settings;
using PingPong_Room_Application.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddPresentation().AddInfrastructure(builder.Configuration).AddApplication();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowAll");
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseExceptionHandler("/error");
app.UseMiddleware<ApiMiddleware>();
app.UseHttpsRedirection();
app.Run();
