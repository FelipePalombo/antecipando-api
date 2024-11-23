namespace WebApi.Models.Entidades;
using WebApi.Models.Interfaces;

using System.Collections.Generic;

public class ResultadoSimulacaoPreDefinida : IResultadoSimulacao
{
    public int IdResultado { get; set; }
    public IBanco MelhorOferta { get; set; }
    public IEnumerable<IBanco> Bancos { get; set; }

    public IBanco GetSimulacaoMelhorOferta()
    {
        return MelhorOferta;
    }

    public IEnumerable<IBanco> GetTodasOfertas()
    {
        return Bancos;
    }
}
