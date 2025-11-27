using ApiPeliculas.Modelo;
using ApiPeliculas.Modelo.Dtos;
using ApiPeliculas.Repositorios.IRepositorios;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ApiPeliculas.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosControllador : ControllerBase
    {
        private readonly IUsuarioRepositorio _sr;
        private readonly IMapper _map;
        protected RespuestaApi _respuestaApi;
        public UsuariosControllador(IUsuarioRepositorio sr, IMapper map)
        {
            _sr = sr;
            _map = map;
            this._respuestaApi = new();
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetPeliculas()
        {
            var listaUsuarios = _sr.GetUsuarios();
            var listausuariosdto = new List<UsuarioDto>();
            foreach (var lista in listaUsuarios)
            {
                listausuariosdto.Add(_map.Map<UsuarioDto>(lista));
            }
            return Ok(listausuariosdto);
        }
        [HttpGet("{UsuarioId:int}", Name = "GetUsuario")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUsuario(int UsuarioId)
        {
            var itemUsuario = _sr.GetUsuario(UsuarioId);

            if (itemUsuario == null)
            {
                return NotFound();
            }
            var itemUsuarioDto = _map.Map<UsuarioDto>(itemUsuario);
            return Ok(itemUsuarioDto);
        }

        [HttpPost("Registro")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Registro([FromBody] RegistroUsuarioDto registroUsuarioDto)
        {
            bool validarUsuario =  _sr.IsUniqueUser(registroUsuarioDto.NombreUsuario);
            if (!validarUsuario)
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
               _respuestaApi.IsSuccess = false;
                _respuestaApi.ErrorMessage.Add("El nombre de usuario ya existe");
                return BadRequest(_respuestaApi);
            }
            var usuario = await _sr.Registro(registroUsuarioDto);
            if (usuario == null)
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.ErrorMessage.Add("Error en el registro");
                return BadRequest(_respuestaApi);
            }
            _respuestaApi.StatusCode = HttpStatusCode.OK;
            _respuestaApi.IsSuccess = true;
            return Ok(_respuestaApi);
        }
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDto usuarioLoginDto)
        {
            var respuestaLogin = await _sr.Login(usuarioLoginDto);
            if (respuestaLogin.Usuario == null || string.IsNullOrEmpty(respuestaLogin.Token))
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.ErrorMessage.Add("El nombre de usuario o password son incorrectas");
                return BadRequest(_respuestaApi);
            }
           
            _respuestaApi.StatusCode = HttpStatusCode.OK;
            _respuestaApi.IsSuccess = true;
            _respuestaApi.Result = respuestaLogin;
            return Ok(_respuestaApi);
        }

    }
}
