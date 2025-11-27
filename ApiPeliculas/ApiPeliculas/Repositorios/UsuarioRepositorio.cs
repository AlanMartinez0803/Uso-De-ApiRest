using ApiPeliculas.Data;
using ApiPeliculas.Modelo;
using ApiPeliculas.Modelo.Dtos;
using ApiPeliculas.Repositorios.IRepositorios;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XSystem.Security.Cryptography;

namespace ApiPeliculas.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly AplicationDbContext _bd;
        private string KeySecret;
        public UsuarioRepositorio(AplicationDbContext bd, IConfiguration config)
        {
            _bd = bd;
            KeySecret = config.GetValue<string>("ApiSetting:Secret");
        }

        public Usuario GetUsuario(int id)
        {
           return _bd.Usuarios.FirstOrDefault(u => u.UsuarioId == id);
        }

        public ICollection<Usuario> GetUsuarios()
        {
           return _bd.Usuarios.OrderBy(C=> C.NombreUsuario).ToList();
        }

        public bool IsUniqueUser(string nombreUsuario)
        {
            var usuarioBd = _bd.Usuarios.FirstOrDefault(u => u.NombreUsuario == nombreUsuario);
            if (usuarioBd == null )
            {
                return true;
            }
            return false;
        }

        public async Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuariologindto)
        {
           var PasswordHash = obtenermd5(usuariologindto.Password);
            var usuarioBd = _bd.Usuarios.FirstOrDefault(u => u.NombreUsuario.ToLower() == usuariologindto.NombreUsuario.ToLower() 
            && u.Password == PasswordHash);
            //Si el usiario no coincide con la bd
            if (usuarioBd == null)
            {
                return new UsuarioLoginRespuestaDto()
                {
                    Token = "",
                    Usuario = null
                };
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KeySecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Claims
            var claims = new Claim[]
            {
            new Claim(ClaimTypes.Name, usuarioBd.NombreUsuario.ToLower()),
            new Claim(ClaimTypes.Role, usuarioBd.Role)
            };

            // Crear token usando JsonWebTokenHandler
            var handler = new JsonWebTokenHandler();
            var token = handler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = creds
            });

            // Preparar la respuesta
            UsuarioLoginRespuestaDto usuarioLoginRespuestaDto = new UsuarioLoginRespuestaDto()
            {
                Token = token,
                Usuario = usuarioBd
            };

            return usuarioLoginRespuestaDto;
        }

        public async Task<Usuario> Registro(RegistroUsuarioDto registroUsuarioDto)
        {
            var PasswordHash = obtenermd5(registroUsuarioDto.Password);
            Usuario usuario = new Usuario()
            {
                NombreUsuario = registroUsuarioDto.NombreUsuario,
                Nombre = registroUsuarioDto.Nombre,
                Password = PasswordHash,
                Role = registroUsuarioDto.Role
            };
            _bd.Usuarios.Add(usuario);
            await _bd.SaveChangesAsync();
            usuario.Password = PasswordHash;
            return usuario;
        }
        public static string obtenermd5(string valor)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(valor);
            bs = x.ComputeHash(bs);
            string respuesta= "";
            for(int i=0;i<bs.Length;i++)
                respuesta += bs[i].ToString("x2").ToLower();
            return respuesta;
        }
    }
}
