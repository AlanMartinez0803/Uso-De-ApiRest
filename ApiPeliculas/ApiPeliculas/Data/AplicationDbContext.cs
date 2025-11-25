using ApiPeliculas.Modelo;
using Microsoft.EntityFrameworkCore;    
namespace ApiPeliculas.Data
{
    public class AplicationDbContext : DbContext
    {
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options)
        {
        }
        //Aqui van los DBSets osea los modelos (Entidades)
        public DbSet<Categoria> Categorias { get; set; }

    }
}
