namespace WebApi.Models.Interfaces;

using System.Collections.Generic;

public interface IResultadoSimulacao
{
    void DefinirMelhorOferta();
    IEnumerable<IBanco> GetTodasOfertas();
}
