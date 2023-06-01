using InmobiliariaV2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InmobiliariaV2.Controllers
{
    [ApiController]
    [Route("api/pagos")]
    //[Authorize]
    public class PagosController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IConfiguration configuration;

        public PagosController(DataContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        [HttpGet("contrato/{contratoId:int}")]
        public async Task<ActionResult<List<Pago>>> GetPagosPorContrato(int contratoId)
        {
            return await context.Pagos
                .Where(p => p.ContratoId == contratoId)
                .ToListAsync();
        }
    }
}
