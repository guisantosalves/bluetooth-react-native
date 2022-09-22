using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alpha.Pesagem.Api.Models;
using Alpha.Pesagem.Api.Services;
using Alpha.Pesagem.Api.Validation;
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

        [HttpPut("Alterar/{id}")]
        public virtual async Task<IActionResult> AlterarAsync([FromBody] Fazenda obj, Guid id)
        {
            var result = await this._service.AlterarAsync(id, obj);

            return Ok(result);
        }

        [HttpGet("ObterVarios")]
        public virtual async Task<IActionResult> ObterVariosAsync()
        {
            var lista = await this._service.ObterVariosAsync();

            return Ok(lista);
        }

        [HttpPost("ObterRegistrosComparados")]
        public virtual async Task<IActionResult> ObterRegistrosComparadosAsync([FromBody] List<ModelComparacaoViewModel> comparacao, [FromQuery] int? diasAnteriores)
        {
            var lista = await this._service.ObterRegistrosComparadosAsync(comparacao, diasAnteriores);

            return Ok(lista);
        }

        [HttpPost("ObterRegistrosRemovidos")]
        public virtual async Task<IActionResult> ObterRegistrosRemovidosAsync(List<Guid> registros)
        {
            var lista = await this._service.ObterRegistrosRemovidosAsync(registros);

            return Ok(lista);
        }

        [HttpGet("ObterUm/{id}")]
        public virtual async Task<IActionResult> ObterUmAsync(Guid id)
        {
            var obj = await this._service.ObterUmAsync(id);

            return Ok(obj);
        }

        [HttpDelete("Remover/{id}")]
        public async Task<IActionResult> RemoverAsync(Guid id)
        {
            await (this._service as FazendaService).RemoverAsync(id);

            return Ok();
        }

        [HttpPost("SalvarEmLoteAlphaExpress")]
        public virtual async Task<IActionResult> SalvarEmLoteAlphaExpressAsync([FromBody] IEnumerable<Fazenda> lista)
        {
            var validator = new FazendaSaveEmLoteValidator();
            var validationResult = await validator.ValidateAsync(lista);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            await this._service.SalvarEmLoteAlphaExpressAsync(lista);
            return Ok();
        }

        [HttpPost("SalvarEmLote")]
        public virtual async Task<IActionResult> SalvarEmLoteAsync([FromBody] IEnumerable<Fazenda> lista)
        {
            var validator = new FazendaSaveEmLoteValidator();
            var validationResult = await validator.ValidateAsync(lista);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            await this._service.SalvarEmLoteAsync(lista);
            return Ok();
        }

        [HttpPost("MarcarFlagSincronizadoEmLote")]
        public async Task<IActionResult> MarcarFlagSincronizadoEmLoteAsync(IEnumerable<KeyValuePair<Guid, int>> lista)
        {
            await this._service.MarcarFlagSincronizadoEmLoteAsync(lista);
            return Ok();
        }
    }
}