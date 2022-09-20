using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Alpha.Pesagem.Api.Models;
using Alpha.Pesagem.Api.Services;

namespace Alpha.Pesagem.Api.Controllers.BaseControllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public abstract class TenantController<T> : ControllerBase where T : EntidadeTenant
    {
        protected ITenantDataService<T> _service;
        public TenantController(ITenantDataService<T> service, IHttpContextAccessor context)
        {
            _service = service;
            var tenantContext = context.HttpContext.GetTenantContext<Empresa>();

            if (tenantContext != null)
            {
                _service.Empresa = tenantContext.Tenant;
            }
        }

        [HttpPost("Salvar")]
        public virtual async Task<IActionResult> SaveAsync([FromBody] T obj)
        {
            var result = await this._service.SalvarAsync(obj);

            return Ok(result);
        }

        [HttpPost("SalvarEmLoteAlphaExpress")]
        public virtual async Task<IActionResult> SalvarEmLoteAlphaExpressAsync([FromBody] IEnumerable<T> lista)
        {
            await this._service.SalvarEmLoteAlphaExpressAsync(lista);

            return Ok();
        }

        [HttpPost("SalvarEmLote")]
        public virtual async Task<IActionResult> SalvarEmLoteAsync([FromBody] IEnumerable<T> lista)
        {
            await this._service.SalvarEmLoteAsync(lista);

            return Ok();
        }

        [HttpPost("Incluir")]
        public virtual async Task<IActionResult> IncluirAsync([FromBody] T obj)
        {
            var result = await this._service.IncluirAsync(obj);

            return Ok(result);
        }

        [HttpPut("Alterar/{id}")]
        public virtual async Task<IActionResult> AlterarAsync([FromBody] T obj, Guid id)
        {
            var result = await this._service.AlterarAsync(id, obj);

            return Ok(result);
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
    }
}
