using System;

namespace Alpha.Pesagem.Api.Models
{
  public class Configuracao : EntidadeTenant, IAlphaExpressRef, IDateLog
  {
    public int? IdAlphaExpress { get; set; }

    public bool UsarTabelaPrecos { get; set; }

    public bool RestringirEstoque { get; set; }

    public bool AlterarClientes { get; set; }

    public bool ArrastarParaAdicionar { get; set; }

    public bool ClientePorRegiao { get; set; }

    public bool AlterarValorProduto { get; set; }

    public bool CadastrarClientes { get; set; }

    public bool AdicionarAcrescimos { get; set; }

    public bool BloquearImpressaoPdf { get; set; }

    public bool AplicaDescontoComTabelaPreco { get; set; }

    public bool OcultarAlertaFinanceiro { get; set; }

    public bool AdicionarAcrescimoVenda { get; set; }

    public decimal DescontoMaximo { get; set; }

    public decimal AcrescimoMaximo { get; set; }

    public bool OcultarDashboard { get; set; }

    public PeriodoFiltragem PeriodoPadraoEmListagens { get; set; }

    public AplicarDesconto AplicarDesconto { get; set; }

    public DateTime DataCriacao { get; set; }

    public DateTime? DataAlteracao { get; set; }

    public bool AplicarValorAtacado { get; set; }

    public bool AplicarQtdMinimaAtacado { get; set; }
  }
  public enum AplicarDesconto : Int16
  {
    Todos,
    DescontoGeral,
    DescontoItem
  }

  public enum PeriodoFiltragem
  {
    DiaAtual,
    DiaAnterior,
    SemanaAtual,
    SemanaAnterior,
    MesAtual,
    MesAnterior,
    AnoAtual,
    AnoAnterior
  }
}