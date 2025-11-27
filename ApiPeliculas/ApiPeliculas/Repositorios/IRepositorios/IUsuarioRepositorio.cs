using ApiPeliculas.Modelo;
using ApiPeliculas.Modelo.Dtos;

namespace ApiPeliculas.Repositorios.IRepositorios
{
    public interface IUsuarioRepositorio
    {
        ICollection<Usuario> GetUsuarios();
        Usuario GetUsuario(int id);
        bool IsUniqueUser(string nombreUsuario);
        Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuariologindto);
        Task<Usuario> Registro(RegistroUsuarioDto registroUsuarioDto);

    }
}
