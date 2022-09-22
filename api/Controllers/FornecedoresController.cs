
// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using Alpha.Pesagem.Api.Controllers.BaseControllers;
// using Alpha.Pesagem.Api.Models;
// using Alpha.Pesagem.Api.Services;
// using Alpha.Pesagem.Api.Validation;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;

// namespace Alpha.Pesagem.Api.Controllers
// {
//   [ApiController]
//   [Route("api/[controller]")]
//   public class FornecedoresController : TenantController<Fornecedor>
//   {
//     public FornecedoresController(ITenantDataService<Fornecedor> service, IHttpContextAccessor context) : base(service, context)
//     {
//     }

//     [HttpPost("Salvar")]
//     public override async Task<IActionResult> SaveAsync([FromBody] Fornecedor obj) 
//     {
//       var validator = new FornecedorSaveValidator();
//       var validationResult = await validator.ValidateAsync(obj);

//       if (!validationResult.IsValid)
//       {
//         return BadRequest(validationResult.Errors);
//       }

//       return await base.SaveAsync(obj);
//     }

//     [HttpPost("Incluir")]
//     public override async Task<IActionResult> IncluirAsync([FromBody] Fornecedor obj)
//     {
//       var validator = new FornecedorSaveValidator();
//       var validationResult = await validator.ValidateAsync(obj);

//       if (!validationResult.IsValid)
//       {
//         return BadRequest(validationResult.Errors);
//       }

//       return await base.IncluirAsync(obj);
//     }

//     [HttpPut("Alterar/{id}")]
//     public override async Task<IActionResult> AlterarAsync([FromBody] Fornecedor obj, Guid id)
//     {
//       var validator = new FornecedorSaveValidator();
//       var validationResult = await validator.ValidateAsync(obj);

//       if (!validationResult.IsValid)
//       {
//         return BadRequest(validationResult.Errors);
//       }

//       return await base.AlterarAsync(obj, id);
//     }

//     [HttpDelete("Remover/{id}")]
//     public override async Task<IActionResult> RemoverAsync(Guid id)
//     {
//       return await base.RemoverAsync(id);
//     }

//     [HttpGet("ObterVarios")]
//     public override async Task<IActionResult> ObterVariosAsync()
//     {
//       return await base.ObterVariosAsync();
//     }

//     [HttpGet("ObterNovos")]
//     public async Task<IActionResult> ObterPedidosNovosAsync()
//     {
//       return Ok(await (this._service as FornecedorService).ObterNovosAsync());
//     }

//     [HttpGet("VerificarCpfOuCnpj")]
//     public async Task<IActionResult> VerificarCpfOuCnpjAsync(string cpfOuCnpj)
//     {
//       var response = new { existe = (await (this._service as FornecedorService).VerificarCpfOuCnpjAsync(cpfOuCnpj)) };
//       return Ok(response);
//     }

//     [HttpGet("ObterUm/{id}")]
//     public override async Task<IActionResult> ObterUmAsync(Guid id)
//     {
//       return await base.ObterUmAsync(id);
//     }

//     [HttpPost("SalvarEmLoteAlphaExpress")]
//     public override async Task<IActionResult> SalvarEmLoteAlphaExpressAsync([FromBody] IEnumerable<Fornecedor> lista)
//     {
//       var validator = new FornecedorSaveEmLoteValidator();
//       var validationResult = await validator.ValidateAsync(lista);

//       if (!validationResult.IsValid)
//       {
//         return BadRequest(validationResult.Errors);
//       }

//       return await base.SalvarEmLoteAlphaExpressAsync(lista);
//     }

//     [HttpPost("SalvarEmLote")]
//     public override async Task<IActionResult> SalvarEmLoteAsync([FromBody] IEnumerable<Fornecedor> lista)
//     {
//       var validator = new FornecedorSaveEmLoteValidator();
//       var validationResult = await validator.ValidateAsync(lista);

//       if (!validationResult.IsValid)
//       {
//         return BadRequest(validationResult.Errors);
//       }

//       return await base.SalvarEmLoteAsync(lista);
//     }

//     [HttpPost("MarcarFlagSincronizadoEmLote")]
//     public async Task<IActionResult> MarcarFlagSincronizadoEmLoteAsync(IEnumerable<KeyValuePair<Guid, int>> lista)
//     {
//       await (this._service as FornecedorService).MarcarFlagSincronizadoEmLoteAsync(lista);
//       return Ok();
//     }
//   }
// }