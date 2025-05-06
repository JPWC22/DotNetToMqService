using RabbitMQ.Client;
using DotNetToMQService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton<IMessageService, MessageService>();
builder.Services.AddSingleton<IConnectionFactory>(sp => new ConnectionFactory()
{
    HostName = "localhost",    // RabbitMQ host (running locally)
    Port = 5672,               // Default RabbitMQ port
    UserName = "guest",        // Default RabbitMQ username
    Password = "guest"         // Default RabbitMQ password
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.MapControllers();

app.Run();