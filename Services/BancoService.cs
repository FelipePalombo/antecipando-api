namespace WebApi.Services;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using WebApi.Helpers;
using WebApi.Models.Entidades;
using WebApi.Models.DTOs;
using WebApi.Models.Interfaces;
using WebApi.Infra;

public interface IBancoService
{
    Task<IBanco> CalcularSimulacaoAsync(SolicitacaoSimulacao solicitacao);
}

public class BancoInterService : IBancoService
{
    private readonly BancoInterClient _bancoInterClient;
    private const int QUANTIDADE_PERIODOS = 10;
    private const string NOME_BANCO = "Banco Inter";
    private readonly IBancoRepositorio _bancoRepositorio;

    public BancoInterService(BancoInterClient bancoInterClient, IBancoRepositorio bancoRepositorio)
    {
        _bancoInterClient = bancoInterClient;
        _bancoRepositorio = bancoRepositorio;
    }

    public async Task<IBanco> CalcularSimulacaoAsync(SolicitacaoSimulacao solicitacao)
    {
        BancoInter bancoInter;
        BancoInfoDTO bancoInfo = _bancoRepositorio.ObterDados(NOME_BANCO);

        try 
        {
            decimal valorLiberado = await _bancoInterClient.SimularAsync(solicitacao.Usuario.GetSaldoFGTS(), QUANTIDADE_PERIODOS);
            bancoInter = new(bancoInfo.IdBanco, bancoInfo.Nome, bancoInfo.UrlBanco, true, null, valorLiberado);
            bancoInter.CalcularValorLiberado();
        } 
        catch (HttpRequestException ex)
        {
            bancoInter = new(bancoInfo.IdBanco, bancoInfo.Nome, bancoInfo.UrlBanco, false, ex.Message, null);
            Console.WriteLine(bancoInter.ToString());
        }
        catch (Exception ex) 
        {
            bancoInter = new(bancoInfo.IdBanco, bancoInfo.Nome, bancoInfo.UrlBanco, false, ex.Message, null);
            Console.WriteLine(bancoInter.ToString());
        }

        return await Task.FromResult(bancoInter);
    }
}

public class BancoPanService : IBancoService
{
    private readonly BancoPanClient _bancoPanClient;
    private readonly IBancoRepositorio _bancoRepositorio;
    private const string NOME_BANCO = "Banco Pan";

    public BancoPanService(BancoPanClient bancoPanClient, IBancoRepositorio bancoRepositorio)
    {
        _bancoPanClient = bancoPanClient;
        _bancoRepositorio = bancoRepositorio;
    }

    public async Task<IBanco> CalcularSimulacaoAsync(SolicitacaoSimulacao solicitacao)
    {
        BancoPan bancoPan;
        BancoInfoDTO bancoInfo = _bancoRepositorio.ObterDados(NOME_BANCO);

        try 
        {
            decimal valorLiberado = await _bancoPanClient.SimularAsync(solicitacao.Usuario.GetSaldoFGTS(), solicitacao.Usuario.GetDataNascimento());
            bancoPan = new(bancoInfo.IdBanco, bancoInfo.Nome, bancoInfo.UrlBanco, true, null, valorLiberado);
            bancoPan.CalcularValorLiberado();
        } 
        catch (HttpRequestException ex)
        {
            bancoPan = new(bancoInfo.IdBanco, bancoInfo.Nome, bancoInfo.UrlBanco, false, ex.Message, null);
            Console.WriteLine(bancoPan.ToString());
        }
        catch (Exception ex) 
        {
            bancoPan = new(bancoInfo.IdBanco, bancoInfo.Nome, bancoInfo.UrlBanco, false, ex.Message, null);
            Console.WriteLine(bancoPan.ToString());
        }

        return await Task.FromResult(bancoPan);
    }
}

public class BancoPagSeguroService : IBancoService
{
    private readonly BancoPagSeguroClient _bancoPagSeguroClient;
    private readonly IBancoRepositorio _bancoRepositorio;
    private const string NOME_BANCO = "Banco PagSeguro";

    public BancoPagSeguroService(BancoPagSeguroClient bancoPagSeguroClient, IBancoRepositorio bancoRepositorio)
    {
        _bancoPagSeguroClient = bancoPagSeguroClient;
        _bancoRepositorio = bancoRepositorio;
    }

    public async Task<IBanco> CalcularSimulacaoAsync(SolicitacaoSimulacao solicitacao)
    {
        BancoPagSeguro bancoPagSeguro;
        BancoInfoDTO bancoInfo = _bancoRepositorio.ObterDados(NOME_BANCO);

        try 
        {   
            string valorLiberadoString = await _bancoPagSeguroClient.SimularAsync(solicitacao.Usuario.GetSaldoFGTS());
            bancoPagSeguro = new(bancoInfo.IdBanco, bancoInfo.Nome, bancoInfo.UrlBanco, true, null, valorLiberadoString);
            bancoPagSeguro.CalcularValorLiberado();
        } 
        catch (HttpRequestException ex)
        {
            bancoPagSeguro = new(bancoInfo.IdBanco, bancoInfo.Nome, bancoInfo.UrlBanco, false, ex.Message, null);
            Console.WriteLine(bancoPagSeguro.ToString());
        }
        catch (Exception ex) 
        {
            bancoPagSeguro = new(bancoInfo.IdBanco, bancoInfo.Nome, bancoInfo.UrlBanco, false, ex.Message, null);
            Console.WriteLine(bancoPagSeguro.ToString());
        }

        return await Task.FromResult(bancoPagSeguro);
    }
}

public class BancoSantanderService : IBancoService
{
    private readonly IBancoRepositorio _bancoRepositorio;
    private const string NOME_BANCO = "Banco Santander";

    public BancoSantanderService(IBancoRepositorio bancoRepositorio){
        _bancoRepositorio = bancoRepositorio;
    }

    public async Task<IBanco> CalcularSimulacaoAsync(SolicitacaoSimulacao solicitacao)
    {
        BancoSantander bancoSantander;
        BancoInfoDTO bancoInfo = _bancoRepositorio.ObterDados(NOME_BANCO);

        try 
        {   
            bancoSantander = new(bancoInfo.IdBanco, bancoInfo.Nome, bancoInfo.UrlBanco, true, null, solicitacao.Usuario.GetSaldoFGTS(), solicitacao.Usuario.GetDataNascimento());
            bancoSantander.CalcularValorLiberado();
        } 
        catch (HttpRequestException ex)
        {
            bancoSantander = new(bancoInfo.IdBanco, bancoInfo.Nome, bancoInfo.UrlBanco, false, ex.Message, null, null);
            Console.WriteLine(bancoSantander.ToString());
        }
        catch (Exception ex) 
        {
            bancoSantander = new(bancoInfo.IdBanco, bancoInfo.Nome, bancoInfo.UrlBanco, false, ex.Message, null, null);
            Console.WriteLine(bancoSantander.ToString());
        }

        return await Task.FromResult(bancoSantander);
    }
}

public class BancoBMGService : IBancoService
{
    private readonly IBancoRepositorio _bancoRepositorio;
    private const string NOME_BANCO = "Banco BMG";
    public BancoBMGService(IBancoRepositorio bancoRepositorio)
    {
        _bancoRepositorio = bancoRepositorio;
    }

    public async Task<IBanco> CalcularSimulacaoAsync(SolicitacaoSimulacao solicitacao)
    {
        BancoBMG bancoBMG;
        BancoInfoDTO bancoInfo = _bancoRepositorio.ObterDados(NOME_BANCO);
        try 
        {   
            bancoBMG = new(bancoInfo.IdBanco, bancoInfo.Nome, bancoInfo.UrlBanco, true, null, solicitacao.Usuario.GetSaldoFGTS(), solicitacao.Usuario.GetDataNascimento());
            bancoBMG.CalcularValorLiberado();
        } 
        catch (HttpRequestException ex)
        {
            bancoBMG = new(bancoInfo.IdBanco, bancoInfo.Nome, bancoInfo.UrlBanco, false, ex.Message, null, null);
            Console.WriteLine(bancoBMG.ToString());
        }
        catch (Exception ex) 
        {
            bancoBMG = new(bancoInfo.IdBanco, bancoInfo.Nome, bancoInfo.UrlBanco, false, ex.Message, null, null);
            Console.WriteLine(bancoBMG.ToString());
        }

        return await Task.FromResult(bancoBMG);
    }
}

public class BancoItauService : IBancoService
{
    private readonly BancoItauClient _bancoItauClient;
    private readonly IBancoRepositorio _bancoRepositorio;
    private const string NOME_BANCO = "Banco Itaú";

    public BancoItauService(BancoItauClient bancoItauClient, IBancoRepositorio bancoRepositorio)
    {
        _bancoItauClient = bancoItauClient;
        _bancoRepositorio = bancoRepositorio;
    }
    public async Task<IBanco> CalcularSimulacaoAsync(SolicitacaoSimulacao solicitacao)
    {
        BancoItau bancoItau;
        BancoInfoDTO bancoInfo = _bancoRepositorio.ObterDados(NOME_BANCO);
        try 
        {
            decimal valorLiberado = await _bancoItauClient.SimularAsync(solicitacao.Usuario.GetSaldoFGTS(), solicitacao.Usuario.GetDataNascimento());
            bancoItau = new(bancoInfo.IdBanco, bancoInfo.Nome, bancoInfo.UrlBanco, true, null, valorLiberado);
            bancoItau.CalcularValorLiberado();
        } 
        catch (HttpRequestException ex)
        {
            bancoItau = new(bancoInfo.IdBanco, bancoInfo.Nome, bancoInfo.UrlBanco, false, ex.Message, null);
            Console.WriteLine(bancoItau.ToString());
        }
        catch (Exception ex) 
        {
            bancoItau = new(bancoInfo.IdBanco, bancoInfo.Nome, bancoInfo.UrlBanco, false, ex.Message, null);
            Console.WriteLine(bancoItau.ToString());
        }

        return await Task.FromResult(bancoItau);
    }
}