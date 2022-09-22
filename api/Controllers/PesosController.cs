using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alpha.Pesagem.Api.Controllers.BaseControllers;
using Alpha.Pesagem.Api.Models;
using Alpha.Pesagem.Api.Services;
using Alpha.Pesagem.Api.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Alpha.Pesagem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PesosController : BaseController<Peso>
    {
        public PesosController(ITenantDataService<Peso> service) : base(service)
        {
        }

        [HttpPost("Salvar")]
        public override async Task<IActionResult> SaveAsync([FromBody] Peso obj)
        {
            var validator = new PesagemSaveValidator();
            var validationResult = await validator.ValidateAsync(obj);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            return await base.SaveAsync(obj);
        }

        [HttpPost("Incluir")]
        public override async Task<IActionResult> IncluirAsync([FromBody] Peso obj)
        {
            var validator = new PesagemSaveValidator();
            var validationResult = await validator.ValidateAsync(obj);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            return await base.IncluirAsync(obj);
        }

        [HttpPut("Alterar/{id}")]
        public override async Task<IActionResult> AlterarAsync([FromBody] Peso obj, Guid id)
        {
            var validator = new PesagemSaveValidator();
            var validationResult = await validator.ValidateAsync(obj);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            return await base.AlterarAsync(obj, id);
        }

        [HttpDelete("Remover/{id}")]
        public override async Task<IActionResult> RemoverAsync(Guid id)
        {
            return await base.RemoverAsync(id);
        }

        [HttpGet("ObterVarios")]
        public override async Task<IActionResult> ObterVariosAsync()
        {
            return await base.ObterVariosAsync();
        }

        [HttpGet("ObterUm/{id}")]
        public override async Task<IActionResult> ObterUmAsync(Guid id)
        {
            return await base.ObterUmAsync(id);
        }

        [HttpPost("SalvarEmLoteAlphaExpress")]
        public override async Task<IActionResult> SalvarEmLoteAlphaExpressAsync([FromBody] IEnumerable<Peso> lista)
        {
            var validator = new PesagemSaveEmLoteValidator();
            var validationResult = await validator.ValidateAsync(lista);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            return await base.SalvarEmLoteAlphaExpressAsync(lista);
        }

        [HttpPost("SalvarEmLote")]
        public override async Task<IActionResult> SalvarEmLoteAsync([FromBody] IEnumerable<Peso> lista)
        {
            var validator = new PesagemSaveEmLoteValidator();
            var validationResult = await validator.ValidateAsync(lista);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            return await base.SalvarEmLoteAsync(lista);
        }

        [HttpPost("MarcarFlagSincronizadoEmLote")]
        public async Task<IActionResult> MarcarFlagSincronizadoEmLoteAsync(IEnumerable<KeyValuePair<Guid, int>> lista)
        {
            await (this._service as PesoService).MarcarFlagSincronizadoEmLoteAsync(lista);
            return Ok();
        }

        [HttpPost("DesmarcarFlagSincronizadoEmLote")]
        public async Task<IActionResult> DesmarcarcarFlagSincronizadoEmLoteAsync(IEnumerable<Guid> lista)
        {
            await (this._service as PesoService).DesmarcarcarFlagSincronizadoEmLoteAsync(lista);
            return Ok();
        }
        [HttpGet("ConsultarHistorico/{dataInicial}/{dataFinal}")]
        public async Task<IActionResult> ConsultarHistoricoAsync(DateTime dataInicial, DateTime dataFinal)
        {
            return Ok(await (this._service as PesoService).ConsultarHistoricoAsync(dataInicial, dataFinal));
        }
    }
}