using System;

namespace Alpha.Pesagem.Api.Models
{
    public class Peso : EntidadeTenant, IDateLog, IAlphaExpressRef
    {
        public string Brinco { get; set; }
        public string BrincoEletronico { get; set; }
        public string PesoTotal { get; set; }
        public string Idade { get; set; }
        public string Raca { get; set; }
        public string ValorMedio { get; set; }
        public string Sexo { get; set; }
        public string Observacao { get; set; }
        public PesagemSincronizada Sincronizado { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int? IdAlphaExpress { get; set; }
    }

    public enum PesagemSincronizada : Int16
    {
        Nao,
        Sim
    }
}