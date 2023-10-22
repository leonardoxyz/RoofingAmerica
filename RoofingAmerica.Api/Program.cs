using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RoofingAmerica.Domain.Models;
using System.Text;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RoofingAmerica.Domain.Core.Data;
using RoofingAmerica.Domain.Models;
using RoofingAmerica.Domain.Services;
using RoofingAmerica.Infrastructure.Data;
using RoofingAmerica.Infrastructure.Repository;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("Homologation");
builder.Services.AddDbContext<DataContext>(opts =>
    opts.UseSqlite(connectionString));

builder.Services.AddScoped<IRepository<Sale, Guid>, SaleRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<SaleService>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "ROOFINGAMERICA-API", Version = "v1" });
    });

    builder.Services.AddSwaggerGen();

    builder.Services.AddEndpointsApiExplorer();
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
