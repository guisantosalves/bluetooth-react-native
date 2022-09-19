using System;
using System.Globalization;
using System.Text;
using Alpha.Pesagem.Api.Data;
using Alpha.Pesagem.Api.Models;
using Alpha.Pesagem.Api.Resolvers;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Alpha.Pesagem.Api
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddCors(options =>
      {
        options.AddDefaultPolicy(builder =>
         {
           builder.AllowAnyOrigin()
             .SetIsOriginAllowedToAllowWildcardSubdomains()
             .AllowAnyMethod()
             .AllowAnyHeader();
         });
      });

      services.AddEntityFrameworkNpgsql().AddDbContext<AlphaDbContext>(options =>
      {
        options.UseNpgsql(Configuration.GetConnectionString("Default"));
      });

      services.AddMultitenancy<Empresa, CachingTenantResolver>();
      services.AddDataServices();
      services.AddHttpContextAccessor();
      ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("pt-BR");

      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
          .AddJwtBearer(options =>
          {
            options.TokenValidationParameters = new TokenValidationParameters
            {
              ValidateIssuer = true,
              ValidateAudience = true,
              ValidateLifetime = true,
              ClockSkew = TimeSpan.FromSeconds(5),
              ValidateIssuerSigningKey = true,
              ValidIssuer = Configuration["Token:Issuer"],
              ValidAudience = Configuration["Token:Audience"],
              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:SigningKey"]))
            };
          });

      services.AddAuthorization();

      services.AddControllers().AddNewtonsoftJson(options =>
      {
        options.UseCamelCasing(true);
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
      });

      // Adicionando o gerador Swagger
      services.AddSwaggerGen((options) =>
      {
        options.SwaggerDoc("v1",
              new OpenApiInfo
              {
                Title = "API - App Pesagem",
                Version = "v1",
                Description = "API - App Pesagem",
                Contact = new OpenApiContact
                {
                  Name = "Equipe DEV - Alpha Software",
                  Email = "desenvolvimento@alphasoftware.com.br",
                  Url = new Uri("https://www.alphasoftware.com.br/contato")
                }
              });
      });
    }


    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (!env.IsProduction())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseCors();

      //Adicionando o middleware para servir o Swagger gerado como um terminal JSON
      app.UseSwagger();

      app.UseSwaggerUI(c =>
      {
        c.RoutePrefix = "docs";
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API - App Pesagem");
      });

      app.UseHttpsRedirection();

      app.UseRouting();
      app.UseStatusCodePages();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseMultitenancy<Empresa>();


      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
