using System;
using System.Threading.Tasks;
using Alpha.Pesagem.Api.Models;
using Alpha.Pesagem.Api.Services;
using Alpha.Pesagem.Api.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Alpha.Pesagem.Api.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class FazendasController : ControllerBase
    {
        private IDataService<Fazenda> _service;
        public FazendasController(IDataService<Fazenda> service)
        {
            _service = service;
        }

        [ApiConsumerFilter]
        [HttpPost("Incluir")]
        public async Task<IActionResult> IncluirAsync([FromBody] Fazenda fazenda)
        {
            var obj = await this._service.IncluirAsync(fazenda);

            return Ok(obj.Id);
        }

        [ApiConsumerFilter]
        [HttpPut("Alterar/{id}")]
        public async Task<IActionResult> AlterarAsync([FromBody] Fazenda fazenda, Guid id)
        {
            var obj = await this._service.AlterarAsync(id, fazenda);

            return Ok(obj.Id);
        }

        [ApiConsumerFilter]
        [HttpGet("ObterVarios")]
        public async Task<IActionResult> ObterVariosAsync()
        {
            var lista = await this._service.ObterVariosAsync();

            return Ok(lista);
        }

        [ApiConsumerFilter]
        [HttpGet("ObterUm/{id}")]
        public async Task<IActionResult> ObterUmAsync(Guid id)
        {
            var obj = await this._service.ObterUmAsync(id);

            return Ok(obj);
        }

        [ApiConsumerFilter]
        [HttpDelete("Remover/{id}")]
        public async Task<IActionResult> RemoverAsync(Guid id)
        {
            await this._service.RemoverAsync(id);

            return Ok();
        }
    }
}