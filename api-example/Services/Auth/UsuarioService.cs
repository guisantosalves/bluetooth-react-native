using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Alpha.Pesagem.Api.Data;
using Alpha.Pesagem.Api.Exceptions;
using Alpha.Pesagem.Api.Models;
using Alpha.Pesagem.Api.Resolvers;
using Alpha.Pesagem.Api.ViewModels;
using JollazCrypt;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Alpha.Pesagem.Api.Services
{
  public class UsuarioService : ReadOnlyDataService<Usuario>
  {
    private IConfiguration _configuration;

    public UsuarioService(AlphaDbContext alphaDbContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(alphaDbContext, httpContextAccessor)
    {
      this._configuration = configuration;
    }

    public List<Claim> GerarUsuarioClaims(Usuario usuario, Guid? subEmpresaId)
    {
      List<Claim> claims = new List<Claim>();

      claims.Add(new Claim(ClaimTypes.Sid, usuario.Id.ToString()));
      claims.Add(new Claim(ClaimTypes.Name, usuario.Nome));
      claims.Add(new Claim(AlphaClaimTypes.TenantNome, this._context.Tenant.Nome));
      claims.Add(new Claim(AlphaClaimTypes.TenantId, this._context.Tenant.Id.ToString()));
      claims.Add(new Claim(ClaimTypes.Role, usuario.Role));

      if (subEmpresaId.HasValue)
      {
        claims.Add(new Claim(AlphaClaimTypes.SubEmpresaId, subEmpresaId.Value.ToString()));
      }

      return claims;
    }

    public async Task<string> ObterSubEmpresaLogadaAsync(Guid? subEmpresaId)
    {
      if (!subEmpresaId.HasValue)
      {
        return "";
      }

      var subEmpresa = await this._context.Set<SubEmpresa>().FirstOrDefaultAsync(q => q.Id == subEmpresaId.Value);

      if (subEmpresa == null)
      {
        return "";
      }

      return subEmpresa.Nome;
    }

    public async Task<Guid?> ObterEmpresaIdAsync(string cnpj)
    {
      var empresa = await this._context.Set<Empresa>().IgnoreQueryFilters().FirstOrDefaultAsync(q => q.Cnpj == cnpj);

      if (empresa == null)
      {
        return null;
      }

      return empresa.Id;
    }

    public async Task<IEnumerable<UsuarioSelectViewModel>> ListarUsuariosEmpresaAsync(Guid tenantId, string roles)
    {
      var usuarios = await this._context.Set<Usuario>()
      .IgnoreQueryFilters()
      .Where(q => q.EmpresaId == tenantId)
      .Where(q => roles.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(q => q.Trim()).Contains(q.Role))
      .Select(q => new UsuarioSelectViewModel { Id = q.Id, Nome = q.Nome })
      .ToListAsync();

      return usuarios;
    }

    public async Task<Usuario> ValidarAsync(Guid id, string senha, Guid empresaId)
    {
      var usuario = await this._context.Usuarios.IgnoreQueryFilters().SingleOrDefaultAsync(u => u.Id == id && u.EmpresaId == empresaId);

      if (usuario == null)
      {
        return null;
      }

      return await ValidarAsync(usuario, senha);
    }

    private async Task<Usuario> ValidarAsync(Usuario usuario, string senha)
    {
      var empresa = await this._context.Empresas.SingleOrDefaultAsync(e => e.Id == usuario.EmpresaId);

      this._context.Tenant = empresa;
      usuario.Empresa = empresa;

      if (senha != null)
      {
        if (!JollazHasherUtil.CompareHashed(HashType.HmacSha1, usuario.Senha, senha, usuario.Salto))
        {
          return null;
        }
      }

      return usuario;
    }
    public async Task<Guid> SaveAsync(Usuario obj)
    {
      if (obj.IdAlphaExpress.HasValue)
      {
        var oldObj = await this._context.Set<Usuario>()
        .IgnoreQueryFilters()
        .AsNoTracking()
        .Where(q => q.IdAlphaExpress == obj.IdAlphaExpress.Value)
        .Where(q => q.EmpresaId == obj.EmpresaId)
        .FirstOrDefaultAsync();

        if (oldObj == null)
          return await this.IncluirAsync(obj);

        return await this.AlterarAsync(obj, oldObj.Id);
      }
      else
      {
        return await this.IncluirAsync(obj);
      }
    }

    public async Task RemoverAsync(Guid id)
    {
      var usuario = await this._context.Set<Usuario>().SingleOrDefaultAsync(q => q.Id == id);

      if (usuario == null)
      {
        throw new AlphaBadRequestException("Usuário não encontrado para a exclusão");
      }

      this._context.Set<Usuario>().Remove(usuario);
      await this._context.SaveChangesAsync();
    }

    public string GerarToken(Usuario usuario, Guid? subEmpresaId)
    {
      if (this._context.Tenant.Id == Guid.Empty)
      {
        throw new Exception("Falha ao obter domínio do token");
      }

      //Setando as principais claims que estarão no token, incluindo o domínio
      var claims = this.GerarUsuarioClaims(usuario, subEmpresaId);

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:SigningKey"]));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      //Criando o token de fato
      var token = new JwtSecurityToken(
          issuer: _configuration["Token:Issuer"],
          audience: _configuration["Token:Audience"],
          claims: claims,
          expires: DateTime.UtcNow.AddDays(10),
          signingCredentials: creds);

      return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<Guid> IncluirAsync(Usuario novoUsuario)
    {
      var novaSenha = JollazHasherUtil.Hash(novoUsuario.Senha, HashType.HmacSha1);

      novoUsuario.Senha = novaSenha.Key;
      novoUsuario.Salto = novaSenha.Value;
      novoUsuario.Inativo = UsuarioInativo.Ativo;

      await this._context.Usuarios.AddAsync(novoUsuario);
      await this._context.SaveChangesAsync();

      return novoUsuario.Id;
    }

    public async Task<Guid> AlterarAsync(Usuario usuario, Guid id)
    {
      var usuarioAtual = await this._context.Set<Usuario>()
      .AsNoTracking()
      .IgnoreQueryFilters()
      .Where(u => u.Id == id)
      .Where(u => u.EmpresaId == usuario.EmpresaId)
      .FirstOrDefaultAsync();

      if (usuarioAtual == null)
      {
        throw new AlphaNotFoundException("Usuário não encontrado!");
      }

      if (string.IsNullOrWhiteSpace(usuario.Senha))
      {
        throw new AlphaBadRequestException("Nova senha não informada!");
      }

      var novaSenhaHash = JollazHasherUtil.Hash(usuario.Senha, HashType.HmacSha1);

      usuarioAtual.Senha = novaSenhaHash.Key;
      usuarioAtual.Salto = novaSenhaHash.Value;
      usuarioAtual.Inativo = usuario.Inativo;
      usuarioAtual.Role = usuario.Role;
      usuarioAtual.Nome = usuario.Nome;
      usuarioAtual.SubEmpresas = usuario.SubEmpresas;
      usuarioAtual.IdAlphaExpress = usuarioAtual.IdAlphaExpress;

      this._context.Update(usuarioAtual);
      await this._context.SaveChangesAsync();

      return usuarioAtual.Id;
    }

    public async Task<Guid> GerarRefreshTokenAsync(Usuario usuario)
    {
      var refreshToken = await this._context
          .Set<RefreshToken>()
          .Where(token => token.UsuarioId == usuario.Id)
          .FirstOrDefaultAsync();

      var token = Guid.NewGuid();

      refreshToken = new RefreshToken { UsuarioId = usuario.Id, Token = token };
      this._context.Add(refreshToken);

      await this.RemoverRefreshTokensAntigosAsync();
      await this._context.SaveChangesAsync();

      return refreshToken.Token;
    }

    private async Task RemoverRefreshTokensAntigosAsync()
    {
      var dataHa90Dias = DateTime.Now - TimeSpan.FromDays(90);
      var registrosAntigos = await this._context.Set<RefreshToken>().Where(q => q.DataCriacao < dataHa90Dias).ToListAsync();

      foreach (var item in registrosAntigos)
      {
        if (item.DataCriacao < dataHa90Dias)
        {
          this._context.Remove(item);
        }
      }
    }

    public async Task<Usuario> ValidarUsuarioDoRefreshTokenAsync(TokenRefreshViewModel tokens)
    {
      var claimsPrincipal = this.ObterClaimsDoTokenExpirado(tokens.ExpiredToken);

      var empresaId = new Guid(claimsPrincipal.Claims.Where(c => c.Type == AlphaClaimTypes.TenantId).SingleOrDefault().Value);
      var usuarioId = new Guid(claimsPrincipal.Claims.Where(c => c.Type == ClaimTypes.Sid).SingleOrDefault().Value);

      var bdRefreshToken = await this._context
      .Set<RefreshToken>()
      .Where(token => token.UsuarioId == usuarioId)
      .Where(token => token.Token == tokens.RefreshToken)
      .SingleOrDefaultAsync();

      if (bdRefreshToken == null) return null;

      return await this._context.Usuarios.IgnoreQueryFilters().SingleOrDefaultAsync(u => u.Id == usuarioId);
    }

    public ClaimsPrincipal ObterClaimsDoTokenExpirado(string token)
    {
      var tokenValidationParameters = new TokenValidationParameters
      {
        ValidAudience = _configuration["Token:Audience"],
        ValidIssuer = _configuration["Token:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:SigningKey"])),
        ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
      };

      var tokenHandler = new JwtSecurityTokenHandler();
      SecurityToken securityToken;
      var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

      var jwtSecurityToken = securityToken as JwtSecurityToken;
      if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        throw new AlphaBadRequestException("Token expirado é inválido");

      return principal;
    }
    public async Task<List<SubEmpresa>> ObterSubEmpresasAsync(Guid usuarioId)
    {
      var usuario = await this._context.Set<Usuario>()
      .IgnoreQueryFilters()
      .FirstOrDefaultAsync(q => q.Id == usuarioId);

      if (usuario == null)
      {
        throw new AlphaBadRequestException("Usuário não encontrado");
      }

      var query = this._context.Set<SubEmpresa>()
      .IgnoreQueryFilters()
      .Where(q => q.EmpresaId == usuario.EmpresaId);

      if (!string.IsNullOrWhiteSpace(usuario.SubEmpresas))
      {
        var subEmpresas = usuario.SubEmpresas.Split(",").Select(q => Convert.ToInt32(q)).ToList();
        query = query.Where(q => subEmpresas.Contains(q.IdAlphaExpress.Value));
      }

      return await query.ToListAsync();
    }
  }
}
