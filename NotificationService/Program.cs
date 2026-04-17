using NotificationService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<INotificationService, InMemoryNotificationService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Notification Service API", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();

public partial class Program { }
