using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alpha.Pesagem.Api;
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
  public class ClientesController : TenantController<Cliente>
  {
    public ClientesController(ITenantDataService<Cliente> service, IHttpContextAccessor context) : base(service, context)
    {
    }

    [HttpPost("Salvar")]
    public override async Task<IActionResult> SaveAsync([FromBody] Cliente obj)
    {
      var validator = new ClienteSaveValidator();
      var validationResult = await validator.ValidateAsync(obj);

      if (!validationResult.IsValid)
      {
        return BadRequest(validationResult.Errors);
      }

      return await base.SaveAsync(obj);
    }

    [HttpPost("Incluir")]
    public override async Task<IActionResult> IncluirAsync([FromBody] Cliente obj)
    {
      var validator = new ClienteSaveValidator();
      var validationResult = await validator.ValidateAsync(obj);

      if (!validationResult.IsValid)
      {
        return BadRequest(validationResult.Errors);
      }

      return await base.IncluirAsync(obj);
    }

    [HttpPut("Alterar/{id}")]
    public override async Task<IActionResult> AlterarAsync([FromBody] Cliente obj, Guid id)
    {
      var validator = new ClienteSaveValidator();
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

    [HttpGet("ObterNovos")]
    public async Task<IActionResult> ObterPedidosNovosAsync()
    {
      return Ok(await (this._service as ClienteService).ObterNovosAsync());
    }

    [HttpGet("VerificarCpfOuCnpj")]
    public async Task<IActionResult> VerificarCpfOuCnpjAsync(string cpfOuCnpj)
    {
      var response = new { existe = (await (this._service as ClienteService).VerificarCpfOuCnpjAsync(cpfOuCnpj)) };
      return Ok(response);
    }

    [HttpGet("ObterUm/{id}")]
    public override async Task<IActionResult> ObterUmAsync(Guid id)
    {
      return await base.ObterUmAsync(id);
    }

    [Authorize(Roles = NivelAcesso.Administrador)]
    [HttpPost("SalvarEmLoteAlphaExpress")]
    public override async Task<IActionResult> SalvarEmLoteAlphaExpressAsync([FromBody] IEnumerable<Cliente> lista)
    {
      var validator = new ClienteSaveEmLoteValidator();
      var validationResult = await validator.ValidateAsync(lista);

      if (!validationResult.IsValid)
      {
        return BadRequest(validationResult.Errors);
      }

      return await base.SalvarEmLoteAlphaExpressAsync(lista);
    }

    [HttpPost("SalvarEmLote")]
    public override async Task<IActionResult> SalvarEmLoteAsync([FromBody] IEnumerable<Cliente> lista)
    {
      var validator = new ClienteSaveEmLoteValidator();
      var validationResult = await validator.ValidateAsync(lista);

      if (!validationResult.IsValid)
      {
        return BadRequest(validationResult.Errors);
      }

      return await base.SalvarEmLoteAsync(lista);
    }

    [Authorize(Roles = NivelAcesso.Administrador)]
    [HttpPost("MarcarFlagSincronizadoEmLote")]
    public async Task<IActionResult> MarcarFlagSincronizadoEmLoteAsync(IEnumerable<KeyValuePair<Guid, int>> lista)
    {
      await (this._service as ClienteService).MarcarFlagSincronizadoEmLoteAsync(lista);
      return Ok();
    }
  }
}