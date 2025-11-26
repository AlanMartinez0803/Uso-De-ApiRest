using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPeliculas.Modelo.Dtos
{
    public class CrearPeliculaDto
    {
      
        public string Nombre { get; set; }
        public string descripcion { get; set; }
        public int Duracion { get; set; }
        public string RutaImagen { get; set; }
        public enum CrearTipoClasificacion { Siete, Trece, Deiciseis, Diesciocho }
        public CrearTipoClasificacion Clasificacion { get; set; }
        public int CategoriaId { get; set; }
       
    }
}
