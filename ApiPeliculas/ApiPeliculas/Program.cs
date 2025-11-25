using ApiPeliculas.Data;
using ApiPeliculas.PeliculaMapper;  
using ApiPeliculas.Repositorios;
using ApiPeliculas.Repositorios.IRepositorios;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSql")));

// Add Repositories
builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();

// Add AutoMapper (esto está bien hecho)
builder.Services.AddAutoMapper(cfg=>
{
    cfg.AddProfile(new PeliculaMapper());
});

builder.Services.AddControllers();

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
