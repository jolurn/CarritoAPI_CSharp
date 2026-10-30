using CarritoAPI.Data;
using CarritoAPI.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 👇 ESTA ES LA LÍNEA MÁGICA PARA QUE SWAGGER FUNCIONE
builder.Services.AddSwaggerGen();

// Contexto (ORM)
builder.Services.AddDbContext<CarritoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositorio (Dapper + EF)
builder.Services.AddScoped<ProductoRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();