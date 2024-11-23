namespace WebApi.Models.Entidades;

using Microsoft.Extensions.Options;
using WebApi.Models.Interfaces;

public class BancoInter : IBanco
{
    public int IdBanco {get; private set;}
    public string Nome {get; private set;}
    public string UrlBanco {get; private set;}
    public decimal? ValorLiberado { get; private set; } = null;
    public bool Disponibilidade { get; set; }
    public string MotivoIndisponbilidade { get; set; }
    public BancoInter(bool disponibilidade, string motivoIndisponbilidade, decimal? valorLiberado)
    {
        IdBanco = 1;
        Nome = "Banco Inter";
        UrlBanco = "https://inter.co/pra-voce/emprestimos/antecipacao-fgts/";
        Disponibilidade = disponibilidade;
        MotivoIndisponbilidade = motivoIndisponbilidade;
        ValorLiberado = valorLiberado;
    }

    public void CalcularValorLiberado()
    {
        if (ValorLiberado == null)
        {
            Disponibilidade = false;
            MotivoIndisponbilidade = "Valor liberado não informado";
        }

        if (ValorLiberado <= 0)
        {
            Disponibilidade = false;
            MotivoIndisponbilidade = "Valor liberado inválido";
        }

        if (ValorLiberado > 0)
        {
            Disponibilidade = true;
        }
    }
}