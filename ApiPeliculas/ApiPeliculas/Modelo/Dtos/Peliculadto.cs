using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPeliculas.Modelo.Dtos
{
    public class Peliculadto
    {
        public int id { get; set; }
        public string Nombre { get; set; }
        public string descripcion { get; set; }
        public int Duracion { get; set; }
        public string RutaImagen { get; set; }
        public enum TipoClasificacion { Siete, Trece, Deiciseis, Diesciocho }
        public TipoClasificacion Clasificacion { get; set; }
        public DateTime FechaCreacion { get; set; }

       
        public int CategoriaId { get; set; }
       
    }
}
