using ApiPeliculas.Data;
using ApiPeliculas.PeliculaMapper;  
using ApiPeliculas.Repositorios;
using ApiPeliculas.Repositorios.IRepositorios;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

//Add Swagger
//Update para que Swagger soporte la autenticación
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer Schema. \r\n\r\n" +
        "Enter 'Bearer' [space] and then your token in the text input below. \r\n\r\n" +
        "Example: \"Bearer 123jriojsadfd\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

// Add services to the container.
builder.Services.AddDbContext<AplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSql")));

//support for caching
//Versiones
var ApiVersion= builder.Services.AddApiVersioning(option =>
{
    option.AssumeDefaultVersionWhenUnspecified = true;
    option.DefaultApiVersion = new ApiVersion(1, 0);
    option.ReportApiVersions = true;
    option.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader("api-version"));
});

ApiVersion.AddApiExplorer(opcion => opcion.GroupNameFormat = "'v'VVV");
// Add Repositories
builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
builder.Services.AddScoped<IPeliculaRepositorio, PeliculaRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
//Add Key for Authentication
var key = builder.Configuration.GetValue<string>("ApiSetting:Secret");

//Add Athentication
builder.Services.AddAuthentication(
    x => {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }
    ).AddJwtBearer( x=>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters 
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            ValidateIssuer = false,
        ValidateAudience = false,
        };
    }) ;
// Add AutoMapper (esto está bien hecho)
builder.Services.AddAutoMapper(cfg=>
{
    cfg.AddProfile(new PeliculaMapper());
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
