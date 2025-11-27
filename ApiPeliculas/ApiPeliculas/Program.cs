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
builder.Services.AddScoped<IPeliculaRepositorio, PeliculaRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
// Add AutoMapper (esto está bien hecho)
builder.Services.AddAutoMapper(cfg=>
{
    cfg.AddProfile(new PeliculaMapper());
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
//Add Swagger
builder.Services.AddSwaggerGen();
// Soporte para Cors
//Ejemplo de dominio: "https://localhost:3223"
//Se puede agregar * para permitir cualquier dominio
builder.Services.AddCors(b => b.AddPolicy("PoliticaCors", build =>
build.WithOrigins("https://localhost:3223").AllowAnyMethod().AllowAnyHeader()
));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("PoliticaCors");
app.UseAuthorization();

app.MapControllers();

app.Run();
