namespace WebApi.Infra;

using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using WebApi.Models.DTOs;

public interface IBancoRepositorio
{
    BancoInfoDTO ObterDados(string nomeBanco);
}

public class BancoRepositorio : IBancoRepositorio
{
    MySqlConnection _connection;
    public BancoRepositorio([FromServices] MySqlConnection connection)
    {
        _connection = connection;
    }
    
    public BancoInfoDTO ObterDados(string nomeBanco)
    {
        string query = "SELECT idBanco, urlBanco, nome FROM bancos WHERE nome = @nomeBanco";
        _connection.Open();
        var bancoInfo = _connection.QueryFirstOrDefault<BancoInfoDTO>(query, new { nomeBanco });
        _connection.Close();
        return bancoInfo;
    }
}
