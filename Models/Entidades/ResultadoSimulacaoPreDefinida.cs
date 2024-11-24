namespace WebApi.Models.Entidades;
using WebApi.Models.Interfaces;

using System.Collections.Generic;

public class ResultadoSimulacaoPreDefinida : IResultadoSimulacao
{
    public SolicitacaoSimulacao Solicitacao { get; set; }
    public IBanco MelhorOferta { get; set; }
    public IEnumerable<IBanco> Bancos { get; set; }

    public ResultadoSimulacaoPreDefinida(SolicitacaoSimulacao solicitacao, IEnumerable<IBanco> bancos)
    {
        Solicitacao = solicitacao;
        Bancos = bancos;
        DefinirMelhorOferta();
    }

    public void DefinirMelhorOferta()
    {
        MelhorOferta = Bancos
            .Where(b => b.ValorLiberado.HasValue)
            .OrderBy(b => b.ValorLiberado.Value)
            .FirstOrDefault();
    }

    public IEnumerable<IBanco> GetTodasOfertas()
    {
        return Bancos;
    }
}