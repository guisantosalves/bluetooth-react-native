using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alpha.Pesagem.Api.Models;
using Alpha.Pesagem.Api.Services;
using Alpha.Pesagem.Api.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Alpha.Pesagem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PesosController : ControllerBase
    {
        private IDataService<Peso> _service;
        public PesosController(IDataService<Peso> service)
        {
            _service = service;
        }

        [HttpPost("Salvar")]
        public virtual async Task<IActionResult> SaveAsync([FromBody] Peso obj)
        {
            var validator = new PesagemSaveValidator();
            var validationResult = await validator.ValidateAsync(obj);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            return Ok(await this._service.SalvarAsync(obj));
        }

        [HttpPost("Incluir")]
        public virtual async Task<IActionResult> IncluirAsync([FromBody] Peso obj)
        {
            var validator = new PesagemSaveValidator();
            var validationResult = await validator.ValidateAsync(obj);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            return Ok(await this._service.IncluirAsync(obj));
        }

        [HttpPut("Alterar/{id}")]
        public virtual async Task<IActionResult> AlterarAsync([FromBody] Peso obj, Guid id)
        {
            var validator = new PesagemSaveValidator();
            var validationResult = await validator.ValidateAsync(obj);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            return Ok(await this._service.AlterarAsync(id, obj));
        }

        [HttpDelete("Remover/{id}")]
        public virtual async Task<IActionResult> RemoverAsync(Guid id)
        {
            await this._service.RemoverAsync(id);
            return Ok();
        }

        [HttpGet("ObterVarios")]
        public virtual async Task<IActionResult> ObterVariosAsync()
        {
            return Ok(await this._service.ObterVariosAsync());
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
            return Ok(await this._service.ObterUmAsync(id));
        }

        [HttpPost("SalvarEmLoteAlphaExpress")]
        public virtual async Task<IActionResult> SalvarEmLoteAlphaExpressAsync([FromBody] IEnumerable<Peso> lista)
        {
            var validator = new PesagemSaveEmLoteValidator();
            var validationResult = await validator.ValidateAsync(lista);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            await this._service.SalvarEmLoteAlphaExpressAsync(lista);
            return Ok();
        }

        [HttpPost("SalvarEmLote")]
        public virtual async Task<IActionResult> SalvarEmLoteAsync([FromBody] IEnumerable<Peso> lista)
        {
            var validator = new PesagemSaveEmLoteValidator();
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

        [HttpPost("DesmarcarFlagSincronizadoEmLote")]
        public async Task<IActionResult> DesmarcarcarFlagSincronizadoEmLoteAsync(IEnumerable<Guid> lista)
        {
            await this._service.DesmarcarcarFlagSincronizadoEmLoteAsync(lista);
            return Ok();
        }
    }
}