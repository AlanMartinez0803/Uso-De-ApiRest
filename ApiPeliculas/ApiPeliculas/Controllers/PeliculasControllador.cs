using ApiPeliculas.Modelo;
using ApiPeliculas.Modelo.Dtos;
using ApiPeliculas.Repositorios.IRepositorios;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace ApiPeliculas.Controllers
{
    [Route("api/peliculas")]
    [ApiController]
    public class PeliculasControllador : ControllerBase
    {
        private readonly IPeliculaRepositorio _pel;
        private readonly IMapper _mapper;
        public PeliculasControllador(IPeliculaRepositorio pel, IMapper mapper)
        {
            _pel = pel;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetPeliculas([FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 10)
        {
            //var listaPeliculas = _pel.GetPeliculas();
            //var ListaPeliculasDto = listaPeliculas.Select(lista => _mapper.Map<Peliculadto>(lista)).ToList();
            //foreach (var lista in listaPeliculas)
            //{
            //    ListaPeliculasDto.Add(_mapper.Map<Peliculadto>(lista));
            //}
            //return Ok(ListaPeliculasDto);
            try
            {
                var totalPeliculas = _pel.GetTotalPeliculas();
                var listaPeliculas = _pel.GetPeliculas(PageNumber, PageSize);
                if (listaPeliculas == null || !listaPeliculas.Any())
                {
                    return NotFound("No se encontraron peliculas");
                }
                var peliculasDto = listaPeliculas.Select(pelicula => _mapper.Map<Peliculadto>(pelicula));
                var response = new
                {
                    pagenumber = PageNumber,
                    TamañoPagina = PageSize,
                    TotalPeliculas = (int)Math.Ceiling(totalPeliculas / (double)PageSize),
                    TotalItems = totalPeliculas,
                    item = peliculasDto
                };

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos de la aplicacion");

            }
        }
            [HttpGet("{Peliculaid:int}", Name = "GetPelicula")]
            [ProducesResponseType(StatusCodes.Status403Forbidden)]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public IActionResult GetPelicula(int Peliculaid)
            {
                try
                {
                    var itemPelicula = _pel.GetPelicula(Peliculaid);

                    if (itemPelicula == null)
                    {
                        return NotFound($"No se encontraron peliculas con ese respectivo ID: {Peliculaid}");
                    }
                    var itemPeliculaDto = _mapper.Map<Peliculadto>(itemPelicula);
                    return Ok(itemPeliculaDto);
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos de la aplicacion");
                }

            }
            [HttpPost]
            [ProducesResponseType(201, Type = typeof(Peliculadto))]
            [ProducesResponseType(StatusCodes.Status201Created)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
            public IActionResult CreatePelicula([FromBody] CrearPeliculaDto peliculaDto)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (peliculaDto == null)
                {
                    return BadRequest(ModelState);
                }
                if (_pel.PeliculaExisteNombre(peliculaDto.Nombre))
                {
                    ModelState.AddModelError("", "La Pelicula ya existe");
                    return StatusCode(404, ModelState);
                }
                var pelicula = _mapper.Map<Pelicula>(peliculaDto);
                if (!_pel.CrearPelicula(pelicula))
                {
                    ModelState.AddModelError("", $"Algo salio mal al guardar esta pelicula {pelicula.Nombre} ");
                }
                return CreatedAtRoute("GetPelicula", new { Peliculaid = pelicula.id }, pelicula);

            }
            [HttpPut("{PeliculaId:int}", Name = "ActualizarPutPelicula")]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            public IActionResult ActualizarPutPelicula(int PeliculaId, [FromBody] Peliculadto peliculadto)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (peliculadto == null || PeliculaId != peliculadto.id)
                {
                    return BadRequest(ModelState);
                }
                var peliculaExistente = _pel.GetPelicula(PeliculaId);
                if (peliculaExistente == null)
                {
                    return NotFound($"No se encontro la categoria: {PeliculaId}");
                }
                var pelicula = _mapper.Map<Pelicula>(peliculadto);
                if (!_pel.ActualizarPelicula(pelicula))
                {
                    ModelState.AddModelError("", $"Algo salio mal al actualizar la pelicula {pelicula.Nombre} ");
                    return StatusCode(500, ModelState);
                }
                return NoContent();
            }
            [HttpDelete("{peliculaid:int}", Name = "BorrarPelicula")]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            public IActionResult BorrarPelicula(int peliculaid)
            {

                if (!_pel.PeliculaExiste(peliculaid))
                {
                    return NotFound();
                }
                var pelicula = _pel.GetPelicula(peliculaid);
                if (!_pel.BorrarPelicula(pelicula))
                {
                    ModelState.AddModelError("", $"Algo salio mal al borrar la pelicula {pelicula.Nombre} ");
                    return StatusCode(500, ModelState);
                }
                return NoContent();
            }

            [HttpGet("GetPeliculaEnCategoria /{categoriaid:int}")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public IActionResult GetPeliculaEnCategoria(int categoriaid)
            {
                try
                {
                    var listaPeliculas = _pel.GetPeliculasEnCategoria(categoriaid);
                    if (listaPeliculas == null || !listaPeliculas.Any())
                    {
                        return NotFound($"No se encontraron peliculas en la categoria con ID: {categoriaid}");
                    }
                    var ListaPeliculasDto = listaPeliculas.Select(pelicula => _mapper.Map<Peliculadto>(pelicula)).ToList();
                    //foreach(var pelicula in listaPeliculas)
                    //{
                    //    ListaPeliculasDto.Add(_mapper.Map<Peliculadto>(pelicula));
                    //}
                    return Ok(ListaPeliculasDto);
                }
                catch (Exception) {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

            }
        }
    }


