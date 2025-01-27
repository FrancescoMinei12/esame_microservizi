using Ordini.Business;
using Ordini.Business.Abstractions;
using Ordini.Repository;
using Ordini.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;
using Inventario.ClientHttp;
using Inventario.ClientHttp.Abstraction;
using Pagamenti.ClientHttp;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000);
});

// Add services to the container.

IConfigurationSection confSectionInventario = builder.Configuration.GetSection(InventarioClientOptions.SectionName);
InventarioClientOptions optionsInventario = confSectionInventario.Get<InventarioClientOptions>() ?? new();
builder.Services.AddHttpClient<Inventario.ClientHttp.Abstraction.IClientHttp, ClientHttp>("InventarioClient",client =>
{
    client.BaseAddress = new Uri("http://inventario:5000");
});

IConfigurationSection confSectionPagamenti = builder.Configuration.GetSection(InventarioClientOptions.SectionName);
InventarioClientOptions optionsPagamenti = confSectionPagamenti.Get<InventarioClientOptions>() ?? new();
builder.Services.AddHttpClient<Pagamenti.ClientHttp.Abstraction.IClientHttp, PagamentiClientHttp>("PagamentiClient", client =>
{
    client.BaseAddress = new Uri("http://pagamenti:5000");
});

string connectionString = "Server=mssql-server;Database=Ordini;User Id=sa;Password=p4ssw0rD;Encrypt=False";
builder.Services.AddDbContext<OrdiniDbContext>(p => p.UseSqlServer(connectionString));

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IBusiness, Business>();

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
