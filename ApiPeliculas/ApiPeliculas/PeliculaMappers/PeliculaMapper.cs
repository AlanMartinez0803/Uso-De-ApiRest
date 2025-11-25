using ApiPeliculas.Modelo;
using ApiPeliculas.Modelo.Dtos;
using AutoMapper;

namespace ApiPeliculas.PeliculaMapper
{
    public class PeliculaMapper : Profile
    {
        public PeliculaMapper()
        {
            CreateMap<Categoria, CategoriaDto>().ReverseMap();
            CreateMap<Categoria, CreatCategoriaDto>().ReverseMap();
        }
    }
}
