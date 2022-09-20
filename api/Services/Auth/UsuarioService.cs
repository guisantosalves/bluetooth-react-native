using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Alpha.Pesagem.Api.Data;
using Alpha.Pesagem.Api.Models;
using Alpha.Pesagem.Api.Resolvers;
using Alpha.Pesagem.Api.ViewModels;
using Alpha.Vendas.Api.Exceptions;
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

    public List<Claim> GerarUsuarioClaims(Usuario usuario)
    {
      List<Claim> claims = new List<Claim>();

      claims.Add(new Claim(ClaimTypes.Sid, usuario.Id.ToString()));
      claims.Add(new Claim(ClaimTypes.Name, usuario.Nome));
      claims.Add(new Claim(AlphaClaimTypes.TenantNome, this._context.Tenant.Nome));
      claims.Add(new Claim(AlphaClaimTypes.TenantId, this._context.Tenant.Id.ToString()));

      return claims;
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

    public async Task<IEnumerable<UsuarioSelectViewModel>> ListarUsuariosEmpresaAsync(Guid tenantId)
    {
      var usuarios = await this._context.Set<Usuario>()
      .IgnoreQueryFilters()
      .Where(q => q.EmpresaId == tenantId)
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

    public string GerarToken(Usuario usuario)
    {
      if (this._context.Tenant.Id == Guid.Empty)
      {
        throw new Exception("Falha ao obter domínio do token");
      }

      //Setando as principais claims que estarão no token, incluindo o domínio
      var claims = this.GerarUsuarioClaims(usuario);

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
  }
}
