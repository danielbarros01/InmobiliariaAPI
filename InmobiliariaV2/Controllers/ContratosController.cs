using InmobiliariaV2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

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

        [HttpGet("vigentes")]
        public async Task<ActionResult<List<Contrato>>> GetContratosVigentes()
        {
            DateTime fechaActual = DateTime.Now;
            string userId = User.FindFirstValue("Id");

            return await context.Contratos
                .Include(c => c.Inquilino)
                .Include(c => c.Inmueble)
                .Where(c => c.Inmueble.PropietarioId == int.Parse(userId))
                .Where(c => c.Desde <= fechaActual && c.Hasta >= fechaActual)
                .ToListAsync();
        }

        [HttpGet("{contratoId:int}/inquilino")]
        public async Task<ActionResult<Inquilino>> GetContratosVigentes(int contratoId)
        {
            DateTime fechaActual = DateTime.Now;

            Contrato contrato = await context.Contratos
                .Include(c => c.Inquilino)
                .Where(c => c.Id == contratoId)
                .Where(c => c.Desde <= fechaActual && c.Hasta >= fechaActual)
                .FirstOrDefaultAsync();

            if (contrato == null)
            {
                return BadRequest("El contrato que busca al inquilino es nulo");
            }

            return contrato.Inquilino;
        }

        [HttpGet("{contratoId:int}/pagos")]
        public async Task<ActionResult<List<Pago>>> GetPagosPorContrato(int contratoId)
        {
            return await context.Pagos
                .Include(p => p.contrato)
                .Where(p => p.ContratoId == contratoId)
                .ToListAsync();
        }
    }
}
