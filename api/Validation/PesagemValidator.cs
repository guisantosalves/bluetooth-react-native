using System.Collections.Generic;
using Alpha.Pesagem.Api.Models;
using FluentValidation;

namespace Alpha.Pesagem.Api.Validation
{
    public class PesagemSaveValidator : AbstractValidator<Peso>
    {
        public PesagemSaveValidator()
        {
            RuleFor(q => q.PesoTotal).NotNull();
            RuleFor(q => q.FazendaId).NotNull();
            RuleFor(q => q.Brinco).MaximumLength(22).NotNull();
            RuleFor(q => q.BrincoEletronico).MaximumLength(22).NotNull();
            RuleFor(q => q.Idade).NotNull();
            RuleFor(q => q.Raca).NotNull();
            RuleFor(q => q.Sexo).NotNull();
            RuleFor(q => q.ValorMedio).NotNull();
            RuleFor(q => q.Observacao).MaximumLength(60);
            RuleFor(q => q.Movimentacao).NotNull();
            RuleFor(q => q.PesagemManual).NotNull();
        }
    }
    public class PesagemSaveEmLoteValidator : AbstractValidator<IEnumerable<Peso>>
    {
        public PesagemSaveEmLoteValidator()
        {
            RuleForEach(q => q).SetValidator(new PesagemSaveValidator());
        }
    }
}