namespace WebApi.Models.Entidades;

using System;

public class SolicitacaoSimulacao
{
    public int IdSolicitacao { get; private set; }
    public DateTime DataSolicitacao { get; private set; }
    public Cliente Usuario { get; private set; }

    public SolicitacaoSimulacao(Cliente usuario)
    {
        DataSolicitacao = DateTime.Now;
        Usuario = usuario;
    }

    public SolicitacaoSimulacao(int idSolicitacao, DateTime dataSolicitacao, Cliente usuario)
    {
        IdSolicitacao = idSolicitacao;
        DataSolicitacao = dataSolicitacao;
        Usuario = usuario;
    }

    public void AtualizarIdSolicitacao(int idSolicitacao)
    {
        IdSolicitacao = idSolicitacao;
    }
}
