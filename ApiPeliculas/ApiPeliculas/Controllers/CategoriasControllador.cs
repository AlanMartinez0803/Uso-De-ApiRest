using ApiPeliculas.Modelo;
using ApiPeliculas.Modelo.Dtos;
using ApiPeliculas.Repositorios.IRepositorios;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasControllador : ControllerBase
    {
        private readonly ICategoriaRepositorio _ct;
        private readonly IMapper _mapper;
        public CategoriasControllador(ICategoriaRepositorio ct, IMapper map)
        {
            _ct = ct;
            _mapper = map;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategorias()
        {
            var listaCategorias = _ct.GetCategorias();
            var listaCategoriasDto = new List<CategoriaDto>();
            foreach (var lista in listaCategorias)
            {
                listaCategoriasDto.Add(_mapper.Map<CategoriaDto>(lista));
            }
            return listaCategoriasDto != null ? Ok(listaCategoriasDto.Count()) : NotFound();
        }

    }
}
