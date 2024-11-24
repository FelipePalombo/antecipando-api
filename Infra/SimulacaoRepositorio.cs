namespace WebApi.Infra;

using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using WebApi.Models.DTOs;
using WebApi.Models.Entidades;
using WebApi.Models.Interfaces;

public interface ISimulacaoRepositorio
{
    int SalvarSolicitacao(SolicitacaoSimulacao solicitacao);
    int SalvarResultado(int idSolicitacao, IBanco banco);
    ResultadoSimulacaoPreDefinida ObterResultado(int idSolicitacao);
}

public class SimulacaoRepositorio : ISimulacaoRepositorio
{
    MySqlConnection _connection;
    public SimulacaoRepositorio([FromServices] MySqlConnection connection)
    {
        _connection = connection;
    }

    public int SalvarSolicitacao(SolicitacaoSimulacao solicitacao)
    {
        var sql = @"
            INSERT INTO Solicitacoes_Simulacao (DataSolicitacao, DataNascimento, SaldoFGTS) 
            VALUES (@DataSolicitacao, @DataNascimento, @SaldoFGTS);
            SELECT LAST_INSERT_ID();";

        var parameters = new
        {
            DataSolicitacao = solicitacao.DataSolicitacao,
            DataNascimento = solicitacao.Usuario.GetDataNascimento(),
            SaldoFGTS = solicitacao.Usuario.SaldoFGTS
        };

        return _connection.QuerySingle<int>(sql, parameters);
    }

    public int SalvarResultado(int idSolicitacao, IBanco banco)
    {
        var sql = @"
            INSERT INTO resultados_simulacao 
            (idSolicitacao, idBanco, valorLiberado, disponibilidade, motivoIndisponibilidade) 
            VALUES 
            (@IdSolicitacao, @IdBanco, @ValorLiberado, @Disponibilidade, @MotivoIndisponibilidade);
            SELECT LAST_INSERT_ID();";

        var parameters = new
        {
            IdSolicitacao = idSolicitacao,
            IdBanco = banco.IdBanco,
            ValorLiberado = banco.ValorLiberado ?? (object)DBNull.Value,
            Disponibilidade = banco.Disponibilidade,
            MotivoIndisponibilidade = banco.MotivoIndisponibilidade?.ToString().Length > 255 
            ? banco.MotivoIndisponibilidade.ToString().Substring(0, 255) 
            : banco.MotivoIndisponibilidade?.ToString() ?? (object)DBNull.Value
        };

        return _connection.QuerySingle<int>(sql, parameters);
    }

    public ResultadoSimulacaoPreDefinida ObterResultado(int idSolicitacao)
    {
        var sql = @"
            SELECT rs.idSolicitacao, ss.dataSolicitacao, ss.dataNascimento, ss.saldoFGTS, rs.idBanco, rs.valorLiberado, rs.disponibilidade, rs.motivoIndisponibilidade, b.nome, b.urlBanco
            FROM resultados_simulacao rs
            INNER JOIN bancos b 
            ON rs.idBanco = b.idBanco
            INNER JOIN solicitacoes_simulacao ss 
            ON ss.idSolicitacao = rs.idSolicitacao
            WHERE rs.idSolicitacao = @IdSolicitacao";

        var resultados = _connection.Query<ResultadoSimulacaoPreDefinidaDTO>(sql, new { IdSolicitacao = idSolicitacao });

        if (!resultados.Any())
        {
            return null;
        }

        var bancos = resultados.Select(r => new Banco
        {
            IdBanco = r.IdBanco,
            Nome = r.Nome,
            UrlBanco = r.UrlBanco,
            ValorLiberado = r.ValorLiberado,
            Disponibilidade = r.Disponibilidade,
            MotivoIndisponibilidade = r.MotivoIndisponibilidade
        }).ToList();

        var solicitacao = new SolicitacaoSimulacao(resultados.First().IdSolicitacao, resultados.First().DataSolicitacao, new Cliente(resultados.First().DataNascimento, resultados.First().SaldoFGTS));

        return new ResultadoSimulacaoPreDefinida(solicitacao, bancos);
    }
}
