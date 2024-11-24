namespace WebApi.Models.Entidades;

using System.Collections.Generic;
using WebApi.Models.Interfaces;

public class ResultadoSimulacao : IResultadoSimulacao
{
    public int IdSolicitacao { get; set; }
    public IBanco MelhorOferta { get; set; }
    public IEnumerable<IBanco> Bancos { get; set; }

    public ResultadoSimulacao(int idSolicitacao)
    {
        IdSolicitacao = idSolicitacao;
        Bancos = new List<IBanco>();
    }

    public void AdicionarNovoBanco(IBanco banco)
    {
        ((List<IBanco>)Bancos).Add(banco);
    }

    public void DefinirMelhorOferta()
    {
        MelhorOferta = Bancos
            .Where(b => b.ValorLiberado.HasValue)
            .OrderByDescending(b => b.ValorLiberado.Value)
            .FirstOrDefault();
    }

    public IEnumerable<IBanco> GetTodasOfertas()
    {
        return Bancos;
    }

    public string GerarLinkCompartilhamento()
    {
        return $"https://antecipando.com.br/simulacao/{IdSolicitacao}";
    }
}
