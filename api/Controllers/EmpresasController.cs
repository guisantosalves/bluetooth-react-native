using System;
using System.Threading.Tasks;
using Alpha.Pesagem.Api.Models;
using Alpha.Pesagem.Api.Services;
using Alpha.Pesagem.Api.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Alpha.Vendas.Api.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class EmpresasController : ControllerBase
    {
        private IDataService<Empresa> _service;
        public EmpresasController(IDataService<Empresa> service)
        {
            _service = service;
        }

        [ApiConsumerFilter]
        [HttpPost("Incluir")]
        public async Task<IActionResult> IncluirAsync([FromBody] Empresa empresa)
        {
            var obj = await this._service.IncluirAsync(empresa);

            return Ok(obj.Id);
        }

        [ApiConsumerFilter]
        [HttpPut("Alterar/{id}")]
        public async Task<IActionResult> AlterarAsync([FromBody] Empresa empresa, Guid id)
        {
            var obj = await this._service.AlterarAsync(id, empresa);

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
        [HttpGet("ObterPorCnpj/{cnpj}")]
        public async Task<IActionResult> ObterPorCnpjAsync(string cnpj)
        {
            var obj = await ((EmpresaService)this._service).ObterPorCnpjAsync(cnpj);

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