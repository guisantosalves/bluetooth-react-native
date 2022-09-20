using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Alpha.Pesagem.Api.Models
{
  public class Fornecedor : EntidadeTenant, IAlphaExpressRef, IUsuarioLog, IDateLog
  {
    public int? IdAlphaExpress { get; set; }

    [Required]
    [StringLength(60, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
    public string NomeRazao { get; set; }

    [StringLength(60, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
    public string Apelido { get; set; }

    public TipoPessoa TipoPessoa { get; set; }

    [Required]
    [StringLength(14, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
    public string Cpf { get; set; }

    [StringLength(20, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
    public string Rg { get; set; }

    [StringLength(9, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
    public string Cep { get; set; }

    [Required]
    [StringLength(60, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
    public string Endereco { get; set; }

    [StringLength(25, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
    public string Bairro { get; set; }

    [StringLength(10, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
    public string Numero { get; set; }

    [StringLength(60, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
    public string Complemento { get; set; }

    [StringLength(35, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
    public string Telefone { get; set; }

    [StringLength(35, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
    public string Celular { get; set; }

    [StringLength(15, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
    public string Contato { get; set; }

    [StringLength(350, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
    public string Observacao { get; set; }

    [StringLength(35, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
    public string Inativo { get; set; }

    [StringLength(35, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
    public string Bloqueado { get; set; }

    // String de ids Alpha Express de vendedores autorizados a ver o registro
    public string Vendedores { get; set; }

    [NotMapped]
    public List<int> VendedoresLista
    {
      get
      {
        if (string.IsNullOrWhiteSpace(this.Vendedores))
        {
          return new List<int>();
        }

        return this.Vendedores.Split(",").Select(q => Convert.ToInt32(q)).ToList();
      }
    }

    public int? CodigoIbgeCidade { get; set; }

    public FornecedorStatusSincronizado Sincronizado { get; set; }

    public Guid? UsuarioCriacaoId { get; set; }

    public virtual Usuario UsuarioCriacao { get; set; }

    public Guid? UsuarioAlteracaoId { get; set; }

    public virtual Usuario UsuarioAlteracao { get; set; }

    public DateTime DataCriacao { get; set; }

    public DateTime? DataAlteracao { get; set; }
    
    public string Aviso { get; set; }
  }


  public enum FornecedorStatusSincronizado : Int16
  {
    CadastradoApp,
    CadastradoAlpha
  }
}

