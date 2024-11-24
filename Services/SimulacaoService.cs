namespace WebApi.Services;

using AutoMapper;
using WebApi.Helpers;
using WebApi.Models.Entidades;
using WebApi.Models.Interfaces;

public interface ISimulacaoService
{
    Task<IResultadoSimulacao> Simular(decimal saldo, DateTime dataNasc);
}

public class SimulacaoService : ISimulacaoService
{
    private readonly IMapper _mapper;
    private IEnumerable<IBancoService> _bancoServices;

    public SimulacaoService(
        IMapper mapper,
        IEnumerable<IBancoService> bancoServices)
    {
        _mapper = mapper;
        _bancoServices = bancoServices;
    }

    public async Task<IResultadoSimulacao> Simular(decimal saldo, DateTime dataNasc)
    {
        SolicitacaoSimulacao solicitacao = new SolicitacaoSimulacao(new Cliente(dataNasc, saldo));
        // SalvarSolicitacao(solicitacao);

        var resultados = new ResultadoSimulacao();

        foreach (var bancoService in _bancoServices)
        {
            var resultado = await bancoService.CalcularSimulacaoAsync(solicitacao);
            resultados.AdicionarNovoBanco(resultado);
        }

        return resultados;
    }

    // private void SalvarSolicitacao(SolicitacaoSimulacao solicitacao)
    // {
    //     _context.SolicitacoesSimulacao.Add(solicitacao);
    //     _context.SaveChanges();
    // }
}