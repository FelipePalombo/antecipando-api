namespace WebApi.Models.Entidades;
using WebApi.Models.Interfaces;

public class BancoPan : IBanco
{
    public int IdBanco {get; private set;}
    public string Nome {get; private set;}
    public string UrlBanco {get; private set;}
    public decimal? ValorLiberado { get; private set; } = null;
    public bool Disponibilidade { get; set; }
    public string MotivoIndisponbilidade { get; set; }
    public BancoPan(bool disponibilidade, string motivoIndisponbilidade, decimal? valorLiberado)
    {
        IdBanco = 2;
        Nome = "Banco Pan";
        UrlBanco = "https://www.bancopan.com.br/produtos/emprestimo/emprestimo-fgts/";
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
