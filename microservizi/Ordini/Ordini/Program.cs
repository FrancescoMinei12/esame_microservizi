using Ordini.Business;
using Ordini.Business.Abstractions;
using Ordini.Repository;
using Ordini.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;
using Pagamenti.ClientHttp;
using Pagamenti.ClientHttp.Abstraction;
using Inventario.ClientHttp;
using Inventario.ClientHttp.Abstraction;
using Ordini.Business.Services;
using Ordini.Shared.Configurations;
using Ordini.BackgroundServices;
using Pagamenti.ClientHttp.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole(); 
builder.Logging.SetMinimumLevel(LogLevel.Debug); 

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000);
});

// Add services to the container.
builder.Services.AddPagamentiClient(builder.Configuration);
builder.Services.AddInventarioClient(builder.Configuration);

string connectionString = "Server=mssql-server;Database=Ordini;User Id=sa;Password=p4ssw0rD;Encrypt=False";
builder.Services.AddDbContext<OrdiniDbContext>(p => p.UseSqlServer(connectionString));
builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IBusiness, Business>();

builder.Services.AddScoped<IOrdiniEventConsumer, OrdiniEventConsumer>();
builder.Services.AddScoped<IOrdineService, OrdineService>();
builder.Services.AddHostedService<OrdiniConsumerBackgroundService>();

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
