using ApiPeliculas.Data;
using ApiPeliculas.Modelo;
using ApiPeliculas.Repositorios.IRepositorios;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Repositorios
{
    public class PeliculaRepositorio : IPeliculaRepositorio
    {
        private readonly AplicationDbContext _bd;
        public PeliculaRepositorio(AplicationDbContext bd)
        {
            _bd = bd;
        }

        public bool ActualizarPelicula(Pelicula pelicula)
        {
           pelicula.FechaCreacion = DateTime.Now;
            var peliculaExistente = _bd.Peliculas.Find(pelicula.id);
            if (peliculaExistente != null)
            {
                _bd.Entry(peliculaExistente).CurrentValues.SetValues(pelicula);
            }
            else 
            {
                _bd.Peliculas.Update(pelicula);
            }
                
            return Guardar();
        }

        public bool BorrarPelicula(Pelicula pelicula)
        {
            _bd.Remove(pelicula);
            return Guardar();
        }

        public IEnumerable<Pelicula> BuscarPeliculas(string nombre)
        {
            IQueryable<Pelicula> query = _bd.Peliculas;
            if (!string.IsNullOrEmpty(nombre))
            {
                query.Where(p => p.Nombre.Contains(nombre) || p.descripcion.Contains(nombre));
            }
                return query.ToList();
        }

        public bool CrearPelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.Now;
            _bd.Peliculas.Add(pelicula);
            return Guardar();
        }

        public Pelicula GetPelicula(int PeliculaId)
        {
            return _bd.Peliculas.FirstOrDefault(p => p.id == PeliculaId);
        }

        //public ICollection<Pelicula> GetPeliculas()
        //{
        //    return _bd.Peliculas.OrderBy(p => p.Nombre).ToList();
        //}
        public ICollection<Pelicula> GetPeliculas(int pageNumber, int pageSize)
        {
            return _bd.Peliculas.OrderBy(p=> p.Nombre.ToLower())
                .Skip((pageNumber-1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
        public int GetTotalPeliculas()
        {
            return _bd.Peliculas.Count();
        }

        public ICollection<Pelicula> GetPeliculasEnCategoria(int CategoriaId)
        {
            return _bd.Peliculas.Include(c => c.Categoria).Where(c => c.CategoriaId==CategoriaId).ToList();
        }

        public bool Guardar()
        {
            return _bd.SaveChanges() >= 0 ? true: false;
        }

        public bool PeliculaExiste(int id)
        {
            bool valor = _bd.Peliculas.Any(p => p.id == id);
            return valor;
        }

        public bool PeliculaExisteNombre(string nombre)
        {
            bool valorExistente = _bd.Peliculas.Any(p => p.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            return valorExistente;
        }

    }
}
