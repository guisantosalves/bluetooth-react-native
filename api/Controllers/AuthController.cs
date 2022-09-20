using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Alpha.Vendas.Api.Exceptions;
using Alpha.Pesagem.Api.Services;
using Alpha.Pesagem.Api.Validation;
using Alpha.Pesagem.Api.Services.Auth;
using Alpha.Pesagem.Api.ViewModels;

namespace Alpha.Pesagem.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private IReadOnlyDataService<Usuario> _service;
        public AuthController(IReadOnlyDataService<Usuario> service)
        {
            _service = service;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Token")]
        public async Task<IActionResult> GerarTokenAsync([FromBody] UsuarioLoginViewModel usuario)
        {
            var validator = new LoginValidator();
            var validationResult = await validator.ValidateAsync(usuario);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var usuarioValidado = await (this._service as UsuarioService).ValidarAsync(usuario.Id, usuario.Senha, usuario.EmpresaId);

            if (usuarioValidado == null)
            {
                return BadRequest("Usuário, senha, ou id da empresa inválidos!");
            }

            var token = (this._service as UsuarioService).GerarToken(usuarioValidado);
            var refreshToken = await (this._service as UsuarioService).GerarRefreshTokenAsync(usuarioValidado);

            return Ok(new { token, userInfo = new { usuarioValidado.Nome, usuarioValidado.IdAlphaExpress, id = usuarioValidado.Id, empresa = usuarioValidado.Empresa.Nome }, refreshToken });
        }

        [AllowAnonymous]
        [ApiConsumerFilter]
        [HttpGet("ObterEmpresaId/{cnpj}")]
        public async Task<IActionResult> ObterEmpresaId(string cnpj)
        {
            Guid? empresaId = await (this._service as UsuarioService).ObterEmpresaIdAsync(cnpj);

            if (!empresaId.HasValue)
            {
                return BadRequest("Não foi encontrada uma empresa para este CPF/CNPJ");
            }

            return Ok(empresaId);
        }

        [AllowAnonymous]
        [ApiConsumerFilter]
        [HttpGet("ListarUsuariosEmpresa/{tenantId}")]
        public async Task<IActionResult> ListarUsuariosEmpresaAsync(Guid tenantId)
        {
            var usuarios = await (this._service as UsuarioService).ListarUsuariosEmpresaAsync(tenantId);

            return Ok(usuarios);
        }

        [AllowAnonymous]
        [ApiConsumerFilter]
        [HttpPost("Incluir")]
        public async Task<IActionResult> IncluirAsync([FromBody] Usuario usuario)
        {
            var validator = new UsuarioSaveValidator();
            var validationResult = await validator.ValidateAsync(usuario);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var id = await (this._service as UsuarioService).IncluirAsync(usuario);

            return Ok(id);
        }

        [AllowAnonymous]
        [ApiConsumerFilter]
        [HttpDelete("Remover/{id}")]
        public async Task<IActionResult> RemoverAsync(Guid id)
        {
            await (this._service as UsuarioService).RemoverAsync(id);

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
            var usuario = await (this._service as UsuarioService).ValidarUsuarioDoRefreshTokenAsync(tokenRefreshViewModel);

            if (usuario == null)
            {
                return BadRequest("O token de atualização não pôde ser validado");
            }

            var claimsAntigas = (this._service as UsuarioService).ObterClaimsDoTokenExpirado(tokenRefreshViewModel.ExpiredToken);

            await (this._service as UsuarioService).ValidarAsync(usuario.Id, usuario.Senha, usuario.EmpresaId);

            var token = (this._service as UsuarioService).GerarToken(usuario);
            var refreshToken = await (this._service as UsuarioService).GerarRefreshTokenAsync(usuario);
            return Ok(new
            {
                primeiroNome = usuario.Nome,
                token = token,
                refreshToken = refreshToken
            });
        }
    }
}
