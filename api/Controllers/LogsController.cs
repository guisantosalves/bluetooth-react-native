using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alpha.Pesagem.Api.Models;
using Alpha.Pesagem.Api.Services;
using Alpha.Pesagem.Api.Validation;
using Microsoft.AspNetCore.Mvc;

namespace Alpha.Pesagem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        private IDataService<Log> _service;
        public LogsController(IDataService<Log> service)
        {
            _service = service;
        }

        [HttpGet("Filtrar/{fazendaId}")]
        public async Task<IActionResult> ObterUmAsync(Guid fazendaId)
        {
            var logs = await (this._service as LogService).Filtrar(fazendaId);
            return Ok(logs);
        }

        [HttpPost("SalvarEmLote")]
        public virtual async Task<IActionResult> SalvarEmLoteAsync([FromBody] IEnumerable<Log> lista)
        {
            var validator = new LogSaveEmLoteValidator();
            var validationResult = await validator.ValidateAsync(lista);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            await (this._service as LogService).SalvarEmLoteAsync(lista);

            return Ok();
        }

        [HttpPost("Salvar")]
        public virtual async Task<IActionResult> SaveAsync([FromBody] Log obj)
        {
            var validator = new LogSaveValidator();
            var validationResult = await validator.ValidateAsync(obj);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            return Ok(await this._service.SalvarAsync(obj));
        }

        [HttpPost("Incluir")]
        public virtual async Task<IActionResult> IncluirAsync([FromBody] Log obj)
        {
            var validator = new LogSaveValidator();
            var validationResult = await validator.ValidateAsync(obj);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            return Ok(await this._service.IncluirAsync(obj));
        }

        [HttpPut("Alterar/{id}")]
        public virtual async Task<IActionResult> AlterarAsync([FromBody] Log obj, Guid id)
        {
            var validator = new LogSaveValidator();
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

        [HttpPost("SalvarEmLoteAlphaExpress")]
        public virtual async Task<IActionResult> SalvarEmLoteAlphaExpressAsync([FromBody] IEnumerable<Log> lista)
        {
            var validator = new LogSaveEmLoteValidator();
            var validationResult = await validator.ValidateAsync(lista);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            await this._service.SalvarEmLoteAlphaExpressAsync(lista);
            return Ok();
        }
    }
}