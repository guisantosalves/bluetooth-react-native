using System;
using System.Threading.Tasks;
using Alpha.Pesagem.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Alpha.Pesagem.Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Alpha.Pesagem.Api.Services.Auth;
using System.Linq;
using Alpha.Pesagem.Api.Validation;
using Alpha.Pesagem.Api.Exceptions;
using Alpha.Pesagem.Api.Resolvers;

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

      var token = (this._service as UsuarioService).GerarToken(usuarioValidado, usuario.SubEmpresaId);
      var refreshToken = await (this._service as UsuarioService).GerarRefreshTokenAsync(usuarioValidado);
      var subEmpresa = await (this._service as UsuarioService).ObterSubEmpresaLogadaAsync(usuario.SubEmpresaId);

      return Ok(new { token, userInfo = new { usuarioValidado.Nome, usuarioValidado.IdAlphaExpress, id = usuarioValidado.Id, empresa = usuarioValidado.Empresa.Nome, subEmpresa = subEmpresa, usuarioValidado.Role }, refreshToken });
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
    public async Task<IActionResult> ListarUsuariosEmpresaAsync(Guid tenantId, string role = NivelAcesso.NaoAdministradores) 
    {
      var usuarios = await (this._service as UsuarioService).ListarUsuariosEmpresaAsync(tenantId, role);

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
    [HttpPost("Salvar")]
    public async Task<IActionResult> SaveAsync([FromBody] Usuario usuario)
    {
      var validator = new UsuarioSaveValidator();
      var validationResult = await validator.ValidateAsync(usuario);

      if (!validationResult.IsValid)
      {
        return BadRequest(validationResult.Errors);
      }

      var id = await (this._service as UsuarioService).SaveAsync(usuario);

      return Ok(id);
    }

    [AllowAnonymous]
    [ApiConsumerFilter]
    [HttpPut("Alterar/{id}")]
    public async Task<IActionResult> AlterarAsync([FromBody] Usuario usuario, Guid id)
    {
      var validator = new UsuarioSaveValidator();
      var validationResult = await validator.ValidateAsync(usuario);

      if (!validationResult.IsValid)
      {
        return BadRequest(validationResult.Errors);
      }

      await (this._service as UsuarioService).AlterarAsync(usuario, id);

      return Ok();
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
    [ApiConsumerFilter]
    [HttpGet("ObterSubEmpresas/{id}")]
    public async Task<IActionResult> ObterSubEmpresasAsync(Guid id)
    {
      var subEmpresas = await (this._service as UsuarioService).ObterSubEmpresasAsync(id);

      return Ok(subEmpresas);
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

      var subEmpresaIdClaim = claimsAntigas.Claims.FirstOrDefault(q => q.Type == AlphaClaimTypes.SubEmpresaId);
      Guid? subEmpresaId = null;

      if (subEmpresaIdClaim != null)
      {
        subEmpresaId = Guid.Parse(subEmpresaIdClaim.Value);
      }

      await (this._service as UsuarioService).ValidarAsync(usuario.Id, usuario.Senha, usuario.EmpresaId);

      var token = (this._service as UsuarioService).GerarToken(usuario, subEmpresaId);
      var refreshToken = await (this._service as UsuarioService).GerarRefreshTokenAsync(usuario);
      return Ok(new
      {
        primeiroNome = usuario.Nome,
        token = token,
        refreshToken = refreshToken
      });
    }
    [AllowAnonymous]
    [HttpPost("Token/RefreshSubEmpresa")]
    public async Task<IActionResult> TokenRefreshSubEmpresaAsync([FromBody] TokenRefreshSubEmpresaViewModel tokenRefreshViewModel)
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

      await (this._service as UsuarioService).ValidarAsync(usuario.Id, usuario.Senha, usuario.EmpresaId);

      var token = (this._service as UsuarioService).GerarToken(usuario, tokenRefreshViewModel.SubEmpresaId);
      var refreshToken = await (this._service as UsuarioService).GerarRefreshTokenAsync(usuario);
      var subEmpresa = await (this._service as UsuarioService).ObterSubEmpresaLogadaAsync(tokenRefreshViewModel.SubEmpresaId);

      return Ok(new
      {
        primeiroNome = usuario.Nome,
        token = token,
        refreshToken = refreshToken,
        subEmpresa
      });
    }
  }
}
