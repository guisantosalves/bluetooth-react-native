using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alpha.Pesagem.Api.Controllers.BaseControllers;
using Alpha.Pesagem.Api.Models;
using Alpha.Pesagem.Api.Services;
using Alpha.Pesagem.Api.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Alpha.Pesagem.Api.Controllers
{
  [ApiController]
  [Authorize(Roles = NivelAcesso.Todos)]
  [Route("api/[controller]")]
  public class LogsController : TenantController<Log>
  {
    public LogsController(ITenantDataService<Log> service, IHttpContextAccessor context) : base(service, context)
    {
    }

    [Authorize(Roles = NivelAcesso.ConsultorVendas)]
    [HttpPost("Salvar")]
    public override async Task<IActionResult> SaveAsync([FromBody] Log obj)
    {
      var validator = new LogSaveValidator();
      var validationResult = await validator.ValidateAsync(obj);

      if (!validationResult.IsValid)
      {
        return BadRequest(validationResult.Errors);
      }

      return await base.SaveAsync(obj);
    }

    [Authorize(Roles = NivelAcesso.ConsultorVendas)]
    [HttpPost("Incluir")]
    public override async Task<IActionResult> IncluirAsync([FromBody] Log obj)
    {
      var validator = new LogSaveValidator();
      var validationResult = await validator.ValidateAsync(obj);

      if (!validationResult.IsValid)
      {
        return BadRequest(validationResult.Errors);
      }

      return await base.IncluirAsync(obj);
    }

    [Authorize(Roles = NivelAcesso.ConsultorVendas)]
    [HttpPut("Alterar/{id}")]
    public override async Task<IActionResult> AlterarAsync([FromBody] Log obj, Guid id)
    {
      var validator = new LogSaveValidator();
      var validationResult = await validator.ValidateAsync(obj);

      if (!validationResult.IsValid)
      {
        return BadRequest(validationResult.Errors);
      }

      return await base.AlterarAsync(obj, id);
    }

    [Authorize(Roles = NivelAcesso.ConsultorVendas)]
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

    [Authorize(Roles = NivelAcesso.Administrador)]
    [HttpGet("Filtrar/{subEmpresaId}/{usuarioId}")]
    public async Task<IActionResult> ObterUmAsync(Guid subEmpresaId, Guid usuarioId)
    {
      var logs = await (this._service as LogService).Filtrar(subEmpresaId, usuarioId);
      return Ok(logs);
    }

    [Authorize(Roles = NivelAcesso.ConsultorVendas)]
    [HttpPost("SalvarEmLoteAlphaExpress")]
    public override async Task<IActionResult> SalvarEmLoteAlphaExpressAsync([FromBody] IEnumerable<Log> lista)
    {
      var validator = new LogSaveEmLoteValidator();
      var validationResult = await validator.ValidateAsync(lista);

      if (!validationResult.IsValid)
      {
        return BadRequest(validationResult.Errors);
      }

      return await base.SalvarEmLoteAlphaExpressAsync(lista);
    }

    [Authorize(Roles = NivelAcesso.ConsultorVendas)]
    [HttpPost("SalvarEmLote")]
    public override async Task<IActionResult> SalvarEmLoteAsync([FromBody] IEnumerable<Log> lista)
    {
      var validator = new LogSaveEmLoteValidator();
      var validationResult = await validator.ValidateAsync(lista);

      if (!validationResult.IsValid)
      {
        return BadRequest(validationResult.Errors);
      }

      return Ok();
    }
  }
}