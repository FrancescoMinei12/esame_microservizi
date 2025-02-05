using Pagamenti.Business;
using Pagamenti.Business.Abstractions;
using Pagamenti.Repository;
using Pagamenti.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;
using Ordini.ClientHttp;
using Ordini.ClientHttp.DependencyInjection;
using Ordini.ClientHttp.Abstraction;
using Pagamenti.Business.Services;
using Pagamenti.BackgroundServices;
using Pagamenti.Shared.Configurations;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000);
});

// Add services to the container.

builder.Services.AddOrdiniClient(builder.Configuration);

string connectionString = "Server=mssql-server;Database=Pagamenti;User Id=sa;Password=p4ssw0rD;Encrypt=False";
builder.Services.AddDbContext<PagamentiDbContext>(p => p.UseSqlServer(connectionString));
builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));


builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IBusiness, Business>();
builder.Services.AddScoped<IPagamentiEventConsumer, PagamentiEventConsumer>();
builder.Services.AddScoped<IPagamentiService, PagamentiService>();

builder.Services.AddHostedService<PagamentiConsumerBackgroundService>();

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
