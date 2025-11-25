using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Modelo.Dtos
{
    public class CategoriaDto
    {
        public int Id { get; set; }
        [Required (ErrorMessage = "Favor de ingresar tu nombre")]
        [MaxLength(100, ErrorMessage = "Sobrepaso el numero maximo de carecteres")]
        public string? Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
