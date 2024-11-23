namespace WebApi.Services;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using WebApi.Helpers;
using WebApi.Models.Entidades;
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

    public BancoInterService(BancoInterClient bancoInterClient)
    {
        _bancoInterClient = bancoInterClient;
    }

    public async Task<IBanco> CalcularSimulacaoAsync(SolicitacaoSimulacao solicitacao)
    {
        BancoInter bancoInter;
        try 
        {
            decimal valorLiberado = await _bancoInterClient.SimularAsync(solicitacao.Usuario.GetSaldoFGTS(), QUANTIDADE_PERIODOS);
            bancoInter = new(true, null, valorLiberado);
            bancoInter.CalcularValorLiberado();
        } 
        catch (HttpRequestException ex)
        {
            bancoInter = new(false, ex.Message, null);
            Console.WriteLine(bancoInter.ToString());
        }
        catch (Exception ex) 
        {
            bancoInter = new(false, ex.Message, null);
            Console.WriteLine(bancoInter.ToString());
        }

        return await Task.FromResult(bancoInter);
    }
}

public class BancoPanService : IBancoService
{
    private readonly BancoPanClient _bancoPanClient;
    public BancoPanService(BancoPanClient bancoPanClient)
    {
        _bancoPanClient = bancoPanClient;
    }

    public async Task<IBanco> CalcularSimulacaoAsync(SolicitacaoSimulacao solicitacao)
    {
        BancoPan bancoPan;
        try 
        {
            decimal valorLiberado = await _bancoPanClient.SimularAsync(solicitacao.Usuario.GetSaldoFGTS(), solicitacao.Usuario.GetDataNascimento());
            bancoPan = new(true, null, valorLiberado);
            bancoPan.CalcularValorLiberado();
        } 
        catch (HttpRequestException ex)
        {
            bancoPan = new(false, ex.Message, null);
            Console.WriteLine(bancoPan.ToString());
        }
        catch (Exception ex) 
        {
            bancoPan = new(false, ex.Message, null);
            Console.WriteLine(bancoPan.ToString());
        }

        return await Task.FromResult(bancoPan);
    }
}

public class BancoPagSeguroService : IBancoService
{
    private readonly BancoPagSeguroClient _bancoPagSeguroClient;
    public BancoPagSeguroService(BancoPagSeguroClient bancoPagSeguroClient)
    {
        _bancoPagSeguroClient = bancoPagSeguroClient;
    }

    public async Task<IBanco> CalcularSimulacaoAsync(SolicitacaoSimulacao solicitacao)
    {
        BancoPagSeguro bancoPagSeguro;
        try 
        {   
            string valorLiberadoString = await _bancoPagSeguroClient.SimularAsync(solicitacao.Usuario.GetSaldoFGTS());
            bancoPagSeguro = new(true, null, valorLiberadoString);
            bancoPagSeguro.CalcularValorLiberado();
        } 
        catch (HttpRequestException ex)
        {
            bancoPagSeguro = new(false, ex.Message, null);
            Console.WriteLine(bancoPagSeguro.ToString());
        }
        catch (Exception ex) 
        {
            bancoPagSeguro = new(false, ex.Message, null);
            Console.WriteLine(bancoPagSeguro.ToString());
        }

        return await Task.FromResult(bancoPagSeguro);
    }
}

public class BancoSantanderService : IBancoService
{
    public BancoSantanderService(){}

    public async Task<IBanco> CalcularSimulacaoAsync(SolicitacaoSimulacao solicitacao)
    {
        BancoSantander bancoSantander;
        try 
        {   
            bancoSantander = new(true, null, solicitacao.Usuario.GetSaldoFGTS(), solicitacao.Usuario.GetDataNascimento());
            bancoSantander.CalcularValorLiberado();
        } 
        catch (HttpRequestException ex)
        {
            bancoSantander = new(false, ex.Message, null, null);
            Console.WriteLine(bancoSantander.ToString());
        }
        catch (Exception ex) 
        {
            bancoSantander = new(false, ex.Message, null, null);
            Console.WriteLine(bancoSantander.ToString());
        }

        return await Task.FromResult(bancoSantander);
    }
}

public class BancoBMGService : IBancoService
{
    public BancoBMGService(){}

    public async Task<IBanco> CalcularSimulacaoAsync(SolicitacaoSimulacao solicitacao)
    {
        BancoBMG bancoBMG;
        try 
        {   
            bancoBMG = new(true, null, solicitacao.Usuario.GetSaldoFGTS(), solicitacao.Usuario.GetDataNascimento());
            bancoBMG.CalcularValorLiberado();
        } 
        catch (HttpRequestException ex)
        {
            bancoBMG = new(false, ex.Message, null, null);
            Console.WriteLine(bancoBMG.ToString());
        }
        catch (Exception ex) 
        {
            bancoBMG = new(false, ex.Message, null, null);
            Console.WriteLine(bancoBMG.ToString());
        }

        return await Task.FromResult(bancoBMG);
    }
}

public class BancoItauService : IBancoService
{
    private readonly BancoItauClient _bancoItauClient;
    public BancoItauService(BancoItauClient bancoItauClient)
    {
        _bancoItauClient = bancoItauClient;
    }
    public async Task<IBanco> CalcularSimulacaoAsync(SolicitacaoSimulacao solicitacao)
    {
        BancoItau bancoItau;
        try 
        {
            decimal valorLiberado = await _bancoItauClient.SimularAsync(solicitacao.Usuario.GetSaldoFGTS(), solicitacao.Usuario.GetDataNascimento());
            bancoItau = new(true, null, valorLiberado);
            bancoItau.CalcularValorLiberado();
        } 
        catch (HttpRequestException ex)
        {
            bancoItau = new(false, ex.Message, null);
            Console.WriteLine(bancoItau.ToString());
        }
        catch (Exception ex) 
        {
            bancoItau = new(false, ex.Message, null);
            Console.WriteLine(bancoItau.ToString());
        }

        return await Task.FromResult(bancoItau);
    }
}