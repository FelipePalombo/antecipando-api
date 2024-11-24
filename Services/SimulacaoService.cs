namespace WebApi.Services;

using AutoMapper;
using WebApi.Helpers;
using WebApi.Infra;
using WebApi.Models.Entidades;
using WebApi.Models.Interfaces;

public interface ISimulacaoService
{
    Task<IResultadoSimulacao> Simular(decimal saldo, DateTime dataNasc);
    IResultadoSimulacao ObterSimulacao(int id);
}

public class SimulacaoService : ISimulacaoService
{
    private readonly IMapper _mapper;
    private IEnumerable<IBancoService> _bancoServices;
    private readonly ISimulacaoRepositorio _simulacaoRepositorio;

    public SimulacaoService(
        IMapper mapper,
        IEnumerable<IBancoService> bancoServices,
        ISimulacaoRepositorio simulacaoRepositorio)
    {
        _mapper = mapper;
        _bancoServices = bancoServices;
        _simulacaoRepositorio = simulacaoRepositorio;
    }

    public async Task<IResultadoSimulacao> Simular(decimal saldo, DateTime dataNasc)
    {
        SolicitacaoSimulacao solicitacao = new SolicitacaoSimulacao(new Cliente(dataNasc, saldo));
        
        int idSolicitacao = _simulacaoRepositorio.SalvarSolicitacao(solicitacao);
        solicitacao.AtualizarIdSolicitacao(idSolicitacao);

        var resultados = new ResultadoSimulacao(idSolicitacao);

        foreach (var bancoService in _bancoServices)
        {
            var resultado = await bancoService.CalcularSimulacaoAsync(solicitacao);
            resultados.AdicionarNovoBanco(resultado);
            _simulacaoRepositorio.SalvarResultado(idSolicitacao, resultado);
        }

        resultados.DefinirMelhorOferta();

        return resultados;
    } 

    public IResultadoSimulacao ObterSimulacao(int id)
    {
        return _simulacaoRepositorio.ObterResultado(id);
    }
}