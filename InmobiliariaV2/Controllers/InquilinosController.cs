using InmobiliariaV2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace InmobiliariaV2.Controllers
{

    [ApiController]
    [Route("api/inquilinos")]
    //[Authorize]
    public class InquilinosController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IConfiguration configuration;

        public InquilinosController(DataContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }


    }
}
