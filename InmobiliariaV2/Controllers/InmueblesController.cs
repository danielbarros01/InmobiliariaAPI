using InmobiliariaV2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InmobiliariaV2.Controllers
{
    [ApiController]
    [Route("api/inmuebles")]
    [Authorize]
    public class InmueblesController : ControllerBase
    {
        private readonly DataContext contexto;
        private readonly IConfiguration configuration;

        public InmueblesController(DataContext contexto, IConfiguration configuration)
        {
            this.contexto = contexto;
            this.configuration = configuration;
        }

        [HttpGet("de")]
        public async Task<ActionResult<List<Inmueble>>> GetInmuebleDePropietario()
        {
            return await contexto.Inmuebles
                //.Include(i => i.Propietario)
                .Include(i => i.TipoInmueble)
                .Where(i => i.Propietario.Email == User.Identity.Name)
                .ToListAsync();
        }

        //No lo utilizo
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Inmueble>> GetInmueble(int id)
        {
            var inmueble = await contexto.Inmuebles
                .Include(i => i.TipoInmueble)
                .FirstOrDefaultAsync(i => i.Id == id);

            if(inmueble == null)
            {
                return BadRequest("El inmueble no existe");
            }

            return Ok(inmueble);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> UpdateStatus(int id, bool disponible)
        {
            var inmueble = await contexto.Inmuebles
                .SingleOrDefaultAsync(i => i.Id == id);

            if (inmueble == null)
            {
                return NotFound("El inmueble no existe");
            }

            inmueble.Disponible = disponible;

            await contexto.SaveChangesAsync();

            return NoContent();
        }
    }
}
