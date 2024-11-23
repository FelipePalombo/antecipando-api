namespace WebApi.Models.Entidades;

using System;

public class SolicitacaoSimulacao
{
    public int IdSolicitacao { get; private set; }
    public DateTime DataSolicitacao { get; private set; }
    public Cliente Usuario { get; private set; }

    public SolicitacaoSimulacao(Cliente usuario)
    {
        IdSolicitacao = new Random().Next(1, int.MaxValue);
        DataSolicitacao = DateTime.Now;
        Usuario = usuario;
    }
}
