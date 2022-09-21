using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Alpha.Pesagem.Api.Data;
using Alpha.Pesagem.Api.Models;
using Alpha.Pesagem.Api.Resolvers;
using Alpha.Pesagem.Api.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using Alpha.Vendas.Api.Exceptions;

namespace Alpha.Pesagem.Api.Services
{
    public class FazendaService : ReadOnlyDataService<Fazenda>
    {
        private IConfiguration _configuration;
        public FazendaService(AlphaDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
            this._configuration = configuration;
        }

        public List<Claim> GerarUsuarioClaims()
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(AlphaClaimTypes.TenantNome, this._context.Tenant.Nome));
            claims.Add(new Claim(AlphaClaimTypes.TenantId, this._context.Tenant.Id.ToString()));

            return claims;

        }
        public async Task<Fazenda> ValidarAsync(LoginViewModel login)
        {
            var fazenda = await this._context.Fazendas.IgnoreQueryFilters().SingleOrDefaultAsync(u => u.Id == login.Id && u.Id == login.FazendaId);

            if (fazenda == null)
            {
                return null;
            }

            return fazenda;
        }

        public string GerarToken(Fazenda fazenda)
        {
            if (this._context.Tenant.Id == Guid.Empty)
            {
                throw new Exception("Falha ao obter domínio do token");
            }

            //Setando as principais claims que estarão no token, incluindo o domínio
            var claims = this.GerarUsuarioClaims();

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

        public async Task<Guid> IncluirAsync(Fazenda novaFazenda)
        {
            novaFazenda.Inativo = FazendaInativa.Ativo;

            await this._context.Fazendas.AddAsync(novaFazenda);
            await this._context.SaveChangesAsync();

            return novaFazenda.Id;
        }

        public async Task<Guid> GerarRefreshTokenAsync(Fazenda fazenda)
        {
            var refreshToken = await this._context
                .Set<RefreshToken>()
                .Where(token => token.FazendaId == fazenda.Id)
                .FirstOrDefaultAsync();

            var token = Guid.NewGuid();

            refreshToken = new RefreshToken { FazendaId = fazenda.Id, Token = token };
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

        public async Task<Fazenda> ValidarFazendaDoRefreshTokenAsync(TokenRefreshViewModel tokens)
        {
            var claimsPrincipal = this.ObterClaimsDoTokenExpirado(tokens.ExpiredToken);

            var fazendaId = new Guid(claimsPrincipal.Claims.Where(c => c.Type == AlphaClaimTypes.TenantId).SingleOrDefault().Value);

            var bdRefreshToken = await this._context
            .Set<RefreshToken>()
            .Where(token => token.Token == tokens.RefreshToken)
            .SingleOrDefaultAsync();

            if (bdRefreshToken == null) return null;

            return await this._context.Fazendas.IgnoreQueryFilters().SingleOrDefaultAsync(u => u.Id == fazendaId);
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

        public async Task RemoverAsync(Guid id)
        {
            var fazenda = await this._context.Set<Fazenda>().SingleOrDefaultAsync(q => q.Id == id);

            if (fazenda == null)
            {
                throw new AlphaBadRequestException("Fazenda não encontrada para a exclusão");
            }

            this._context.Set<Fazenda>().Remove(fazenda);
            await this._context.SaveChangesAsync();
        }

    }
}