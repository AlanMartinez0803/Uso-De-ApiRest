using ApiPeliculas.Modelo;

namespace ApiPeliculas.Repositorios.IRepositorios
{
    public interface IPeliculaRepositorio
    {
        //ICollection<Pelicula> GetPeliculas();
        ICollection<Pelicula> GetPeliculas(int pageNumber, int pageSize);
        int GetTotalPeliculas();
        ICollection<Pelicula> GetPeliculasEnCategoria(int CategoriaId);
        IEnumerable<Pelicula> BuscarPeliculas(string nombre);
        Pelicula GetPelicula(int PeliculaId);
        bool PeliculaExiste(int id);
        bool PeliculaExisteNombre(string nombre);

        bool CrearPelicula(Pelicula pelicula);
        bool ActualizarPelicula(Pelicula pelicula);
        bool BorrarPelicula(Pelicula pelicula);
        bool Guardar();
    }
}
