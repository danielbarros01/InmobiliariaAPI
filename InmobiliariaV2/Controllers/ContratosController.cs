using InmobiliariaV2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InmobiliariaV2.Controllers
{

    [ApiController]
    [Route("api/contratos")]
    [Authorize]
    public class ContratosController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IConfiguration configuration;

        public ContratosController(DataContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        [HttpGet("inmueble/{inmuebleId:int}")]
        public async Task<ActionResult<List<Contrato>>> GetContratosPorInquilino(int inmuebleId)
        {
            return await context.Contratos
                .Include(c => c.Inquilino)
                .Where(c => c.InmuebleId == inmuebleId)
                .ToListAsync();
        }
    }
}
