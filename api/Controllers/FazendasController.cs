using Alpha.Pesagem.Api.Models;
using Alpha.Pesagem.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Alpha.Pesagem.Api.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class FazendasController : ControllerBase
    {
        public FazendasController(DataService<Fazenda> service)
        {
        }
    }
}