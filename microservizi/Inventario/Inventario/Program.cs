using Inventario.Business;
using Inventario.Business.Abstractions;
using Inventario.Business.Kafka;
using Inventario.Repository;
using Inventario.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;
using Utility.Kafka.Abstractions.Clients;
using Utility.Kafka.Clients;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000);
});

// Add services to the container.

string connectionString = "Server=mssql-server;Database=Inventario;User Id=sa;Password=p4ssw0rD;Encrypt=False";
builder.Services.AddDbContext<InventarioDbContext>(p => p.UseSqlServer(connectionString));

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IBusiness, Business>();

builder.Services.AddKafkaProducerService<KafkaTopicsOutput, ProducerService>(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
