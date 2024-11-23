namespace WebApi.Models.Entidades;

using System.Collections.Generic;
using WebApi.Models.Interfaces;

public class ResultadoSimulacao : IResultadoSimulacao
{
    public int IdResultado { get; set; }
    public IBanco MelhorOferta { get; set; }
    public IEnumerable<IBanco> Bancos { get; set; }

    public ResultadoSimulacao()
    {
        Bancos = new List<IBanco>();
    }

    public void AdicionarNovoBanco(IBanco banco)
    {
        ((List<IBanco>)Bancos).Add(banco);
    }

    public IBanco GetSimulacaoMelhorOferta()
    {
        return MelhorOferta;
    }

    public IEnumerable<IBanco> GetTodasOfertas()
    {
        return Bancos;
    }

    public string GerarLinkCompartilhamento()
    {
        return $"https://banco.com/simulacao/{IdResultado}";
    }
}
