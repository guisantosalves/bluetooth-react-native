using System;
using Alpha.Pesagem.Api.Models;

namespace Alpha.Pesagem.Api.ViewModels
{
    public class ConfiguracaoViewModel
    {
       public Configuracao Configuracao { get; set; }
       public UsuarioPermissao UsuarioPermissao { get; set; }
    }
}