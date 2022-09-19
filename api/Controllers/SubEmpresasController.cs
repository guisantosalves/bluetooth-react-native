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
  [Authorize(Roles = NivelAcesso.Administrador)]
  [Route("api/[controller]")]
  public class SubEmpresasController : TenantController<SubEmpresa>
  {
    public SubEmpresasController(ITenantDataService<SubEmpresa> service, IHttpContextAccessor context) : base(service, context)
    {
    }

    [HttpPost("Salvar")]
    public override async Task<IActionResult> SaveAsync([FromBody] SubEmpresa obj)
    {
      var validator = new SubEmpresaSaveValidator();
      var validationResult = await validator.ValidateAsync(obj);

      if (!validationResult.IsValid)
      {
        return BadRequest(validationResult.Errors);
      }

      return await base.SaveAsync(obj);
    }

    [HttpPost("Incluir")]
    public override async Task<IActionResult> IncluirAsync([FromBody] SubEmpresa obj)
    {
      var validator = new SubEmpresaSaveValidator();
      var validationResult = await validator.ValidateAsync(obj);

      if (!validationResult.IsValid)
      {
        return BadRequest(validationResult.Errors);
      }

      return await base.IncluirAsync(obj);
    }

    [HttpPut("Alterar/{id}")]
    public override async Task<IActionResult> AlterarAsync([FromBody] SubEmpresa obj, Guid id)
    {
      var validator = new SubEmpresaSaveValidator();
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
    public override async Task<IActionResult> SalvarEmLoteAlphaExpressAsync([FromBody] IEnumerable<SubEmpresa> lista)
    {
      var validator = new SubEmpresaSaveEmLoteValidator();
      var validationResult = await validator.ValidateAsync(lista);

      if (!validationResult.IsValid)
      {
        return BadRequest(validationResult.Errors);
      }

      return await base.SalvarEmLoteAlphaExpressAsync(lista);
    }

    [HttpPost("SalvarEmLote")]
    public override async Task<IActionResult> SalvarEmLoteAsync([FromBody] IEnumerable<SubEmpresa> lista)
    {
      var validator = new SubEmpresaSaveEmLoteValidator();
      var validationResult = await validator.ValidateAsync(lista);

      if (!validationResult.IsValid)
      {
        return BadRequest(validationResult.Errors);
      }

      return await base.SalvarEmLoteAsync(lista);
    }
  }
}