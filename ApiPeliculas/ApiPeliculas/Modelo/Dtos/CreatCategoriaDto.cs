using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Modelo.Dtos
{
    public class CreatCategoriaDto
    {
       
        [Required(ErrorMessage = "Favor de ingresar tu nombre")]
        [MaxLength(100, ErrorMessage = "Sobrepaso el numero maximo de carecteres")]
        public string? Nombre { get; set; }
    }
}
