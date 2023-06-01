using InmobiliariaV2.Models;
using InmobiliariaV2.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace InmobiliariaV2.Controllers
{
    [ApiController]
    [Route("api/propietarios")]
    public class PropietariosController : ControllerBase
    {
        private readonly DataContext contexto;
        private readonly IConfiguration configuration;

        public PropietariosController(DataContext contexto, IConfiguration configuration)
        {
            this.contexto = contexto;
            this.configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<List<Propietario>>> Get()
        {
            return await contexto.Propietarios.ToListAsync();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginView login)
        {
            Propietario propietario = await contexto.Propietarios.FirstOrDefaultAsync(p => p.Email == login.Email);

            if (propietario == null)
            {
                return BadRequest("Los datos no son validos, verifica el email y la contraseña");
            }

            string hashed = GeneratePasswordHash.GenerateHash(login.Password, configuration["Salt"]);

            if (propietario.Password != hashed)
            {
                return BadRequest("Contraseña Incorrecta");
            }

            //Crea una instancia de SymmetricSecurityKey utilizando la clave secreta especificada en la configuración.
            var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(configuration["TokenAuthentication:SecretKey"]));

            //Crea un objeto SigningCredentials que encapsula la clave de seguridad y el algoritmo de firma utilizado para firmar el token.
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>{
            new Claim(ClaimTypes.Name, propietario.Email),
            new Claim("Id", propietario.Id.ToString()),
            };

            //Crea un objeto JwtSecurityToken que representa el token JWT. Se especifican el emisor, la audiencia, las reclamaciones, el tiempo de expiración y las credenciales de firma.
            var token = new JwtSecurityToken(
                issuer: configuration["TokenAuthentication:Issuer"],
                audience: configuration["TokenAuthentication:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(60),
                signingCredentials: credenciales
            );

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }

        [HttpGet("perfil")]
        [Authorize]
        public async Task<ActionResult> Perfil()
        {
            var propietario = await contexto.Propietarios
                .Where(p => p.Email == User.Identity.Name)
                .Select(p => new PropietarioView(p))
                .FirstOrDefaultAsync();

            if (propietario == null)
            {
                return NotFound();
            }

            return Ok(propietario);
        }

        [HttpPut("perfil")]
        [Authorize]
        public async Task<ActionResult> Perfil(Propietario p)
        {
            var propietario = await contexto.Propietarios
                .Where(p => p.Email == User.Identity.Name)
                .FirstOrDefaultAsync();

            if (propietario == null)
            {
                return BadRequest("No se encontro al propietario");
            }

            propietario.Dni = p.Dni;
            propietario.Nombre = p.Nombre;
            propietario.Apellido = p.Apellido;
            propietario.Telefono = p.Telefono;

            await contexto.SaveChangesAsync();

            var propietarioActualizado = await contexto.Propietarios
                .Where(p => p.Email == User.Identity.Name)
                .Select(p => new PropietarioView(p))
                .FirstOrDefaultAsync();

            return Ok(propietarioActualizado);
        }

        [HttpPut("perfil/password")]
        [Authorize]
        public async Task<ActionResult> Password(PasswordsPropietario passwords)
        {
            var propietario = await contexto.Propietarios
                .Where(p => p.Email == User.Identity.Name)
                .FirstOrDefaultAsync();

            if (propietario == null)
            {
                return BadRequest("No se encontro al propietario");
            }

            if (propietario.Password != GeneratePasswordHash.GenerateHash(passwords.contraseñaActual, configuration["Salt"]))
            {
                return BadRequest("La contraseña actual es incorrecta");
            }

            string hashedNuevaContraseña = GeneratePasswordHash.GenerateHash(passwords.contraseñaNueva, configuration["Salt"]);

            propietario.Password = hashedNuevaContraseña;

            await contexto.SaveChangesAsync();

            return Ok();
        }
    }
}
