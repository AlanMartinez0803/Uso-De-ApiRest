using ApiPeliculas.Modelo;
using ApiPeliculas.Modelo.Dtos;
using ApiPeliculas.Repositorios.IRepositorios;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    [Route("api/categorias")]
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
            return Ok(listaCategoriasDto);
        }
       
        [HttpGet ("{id:int}", Name = "GetCategorias")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategoria(int id)
        {
            var itemCategoria = _ct.GetCategoria(id);

            if (itemCategoria == null)
            {
                return NotFound();
            }
          var itemCategoriaDto = _mapper.Map<CategoriaDto>(itemCategoria);
            return Ok(itemCategoriaDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult CreateCategoria([FromBody] CreatCategoriaDto categoriadto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (categoriadto == null)
            {
                return BadRequest(ModelState);
            }
            if (_ct.CategoriaExisteNombre(categoriadto.Nombre))
            {
                ModelState.AddModelError("", "La categoria ya existe");
                return StatusCode(404, ModelState);
            }
            var categoria = _mapper.Map<Categoria>(categoriadto);
            if (!_ct.CrearCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal al guardar esta categoria {categoria.Nombre} ");
            }
            return CreatedAtRoute("GetCategorias", new { id = categoria.Id }, categoria);

        }
        [HttpPatch("{id:int}", Name = "ActualizarPathCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ActualizarPathCategoria(int id, [FromBody] CategoriaDto categoriadto)
        {
          if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
          if (categoriadto == null || id != categoriadto.Id)
            {
                return BadRequest(ModelState);
            }
            var categoria = _mapper.Map<Categoria>(categoriadto);
            if (!_ct.ActualizarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal al actualizar la categoria {categoria.Nombre} ");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpPut("{id:int}", Name = "ActualizarPutCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ActualizarPutCategoria(int id, [FromBody] CategoriaDto categoriadto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (categoriadto == null || id != categoriadto.Id)
            {
                return BadRequest(ModelState);
            }
            var categoriaExistente = _ct.GetCategoria(id);
            if (categoriaExistente == null)
            {
                return NotFound($"No se encontro la categoria: {id}");
            }
            var categoria = _mapper.Map<Categoria>(categoriadto);
            if (!_ct.ActualizarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal al actualizar la categoria {categoria.Nombre} ");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpDelete("{id:int}", Name = "BorrarCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult BorrarCategoria(int id)
        {
           
           
          
            if (!_ct.CategoriaExiste(id))
            {
                return NotFound();
            }
            var categoria = _ct.GetCategoria(id);
            if (!_ct.BorrarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal al borrar la categoria {categoria.Nombre} ");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
