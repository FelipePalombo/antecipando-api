namespace WebApi.Models.Entidades;
using WebApi.Models.Interfaces;

public class BancoItau : IBanco
{
    public int IdBanco {get; private set;}
    public string Nome {get; private set;}
    public string UrlBanco {get; private set;}
    public decimal? ValorLiberado { get; private set; } = null;
    public bool Disponibilidade { get; set; }
    public string MotivoIndisponbilidade { get; set; }
    public BancoItau(bool disponibilidade, string motivoIndisponbilidade, decimal? valorLiberado)
    {
        IdBanco = 6;
        Nome = "Banco Itaú";
        UrlBanco = "https://emprestimo.itau.com.br/saque-aniversario-fgts/";
        Disponibilidade = disponibilidade;
        MotivoIndisponbilidade = motivoIndisponbilidade;
        ValorLiberado = valorLiberado.HasValue ? Math.Round(valorLiberado.Value, 2) : null;
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
