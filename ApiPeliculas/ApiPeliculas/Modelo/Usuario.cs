using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Modelo
{
    public class Usuario
    {
        [Key]
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public string Nombre { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
