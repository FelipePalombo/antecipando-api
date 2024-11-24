namespace WebApi.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.DTOs;
using WebApi.Services;


[ApiController]
[Route("[controller]")]
public class SimulacoesController : ControllerBase
{
    private readonly ISimulacaoService _simulacaoService;
    private readonly IMapper _mapper;

    public SimulacoesController(
        ISimulacaoService simulacaoService,
        IMapper mapper)
    {
        _simulacaoService = simulacaoService;
        _mapper = mapper;
    }

    // GET /simulacoes?saldoFGTS=1000&dataNascimento=1990-01-01
    [HttpGet]
    public async Task<IActionResult> Simular([FromQuery] SolicitacaoSimulacaoDTO request)
    {
        var simulacao = await _simulacaoService.Simular(request.SaldoFGTS, request.DataNascimento);
        return Ok(simulacao);
    }

    // // GET /simulacoes/{id}
    // [HttpGet("{id}")]
    // public IActionResult GetById(int id)
    // {
    //     var simulacao = _simulacaoService.GetById(id);
    //     if (simulacao == null)
    //         return NotFound(new { message = "Simulação não encontrada" });

    //     return Ok(simulacao);
    // }

    // // POST /simulacoes
    // [HttpPost]
    // public IActionResult Create(SalvarSimulacaoDTO dto)
    // {
    //     var simulacao = _simulacaoService.Create(model);
    //     return Ok(new { message = "Simulação criada com sucesso", simulacao });
    // }

    // // GET /simulacoes/ideais
    // [HttpGet("ideais")]
    // public IActionResult GetSimulacoesIdeais()
    // {
    //     var simulacoesIdeais = _simulacaoService.GetSimulacoesIdeais();
    //     return Ok(simulacoesIdeais);
    // }
}
