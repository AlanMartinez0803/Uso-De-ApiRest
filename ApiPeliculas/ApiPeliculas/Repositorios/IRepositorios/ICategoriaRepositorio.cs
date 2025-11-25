using ApiPeliculas.Modelo;

namespace ApiPeliculas.Repositorios.IRepositorios
{
    public interface ICategoriaRepositorio
    {
        ICollection<Categoria> GetCategorias();
        Categoria GetCategoria(int categoriaId);
        bool CategoriaExiste(int id);
        bool CategoriaExisteNombre(string nombre);

        bool CrearCategoria(Categoria categoria);
        bool ActualizarCategoria(Categoria categoria);
        bool BorrarCategoria(Categoria categoria);
        bool Guardar();
    }
}
