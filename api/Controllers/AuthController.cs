using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Alpha.Pesagem.Api.Services;
using Alpha.Pesagem.Api.Validation;
using Alpha.Pesagem.Api.Services.Auth;
using Alpha.Pesagem.Api.ViewModels;
using Alpha.Pesagem.Api.Models;
using Alpha.Pesagem.Api.Exceptions;

namespace Alpha.Pesagem.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private IDataService<Fazenda> _service;
        public AuthController(IDataService<Fazenda> service)
        {
            _service = service;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Token")]
        public async Task<IActionResult> GerarTokenAsync([FromBody] LoginViewModel login)
        {
            var validator = new LoginValidator();
            var validationResult = await validator.ValidateAsync(login);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var fazendaValidado = await (this._service as FazendaService).ValidarAsync(login);

            if (fazendaValidado == null)
            {
                return BadRequest("Fazenda inválida!");
            }

            var token = (this._service as FazendaService).GerarToken(fazendaValidado);
            var refreshToken = await (this._service as FazendaService).GerarRefreshTokenAsync(fazendaValidado);

            return Ok(new { token, info = new { fazendaValidado.Nome, fazendaValidado.Inativo, fazendaValidado.IdAlphaExpress, fazendaValidado.Id }, refreshToken });
        }

        [AllowAnonymous]
        [ApiConsumerFilter]
        [HttpPost("Incluir")]
        public async Task<IActionResult> IncluirAsync([FromBody] Fazenda fazenda)
        {
            var validator = new LoginSaveValidator();
            var validationResult = await validator.ValidateAsync(fazenda);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var id = await (this._service as FazendaService).IncluirAsync(fazenda);

            return Ok(id);
        }

        [AllowAnonymous]
        [ApiConsumerFilter]
        [HttpDelete("Remover/{id}")]
        public async Task<IActionResult> RemoverAsync(Guid id)
        {
            await (this._service as FazendaService).RemoverAsync(id);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("Token/Refresh")]
        public async Task<IActionResult> TokenRefreshAsync([FromBody] TokenRefreshViewModel tokenRefreshViewModel)
        {
            if (User.Identity.IsAuthenticated)
            {
                throw new AlphaBadRequestException("Headers de autorização não são aceitos nesta requisição.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var fazenda = await (this._service as FazendaService).ValidarFazendaDoRefreshTokenAsync(tokenRefreshViewModel);

            if (fazenda == null)
            {
                return BadRequest("O token de atualização não pôde ser validado");
            }

            var claimsAntigas = (this._service as FazendaService).ObterClaimsDoTokenExpirado(tokenRefreshViewModel.ExpiredToken);

            await (this._service as FazendaService).ValidarAsync(new LoginViewModel { Id = fazenda.Id });

            var token = (this._service as FazendaService).GerarToken(fazenda);
            var refreshToken = await (this._service as FazendaService).GerarRefreshTokenAsync(fazenda);
            return Ok(new
            {
                primeiroNome = fazenda.Nome,
                token = token,
                refreshToken = refreshToken
            });
        }
    }
}
