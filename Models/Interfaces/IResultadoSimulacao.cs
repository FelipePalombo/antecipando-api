namespace WebApi.Models.Interfaces;

using System.Collections.Generic;

public interface IResultadoSimulacao
{
    IBanco GetSimulacaoMelhorOferta();
    IEnumerable<IBanco> GetTodasOfertas();
}
