using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Modelo.Dtos
{
    public class RegistroUsuarioDto
    {
        [Required (ErrorMessage ="El usuario es obligatorio")]
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "El Nombre es obligatorio")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El Password es obligatorio")]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
