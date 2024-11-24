namespace WebApi.Models.Entidades;
using WebApi.Models.Interfaces;

public class BancoItau : IBanco
{
    public int IdBanco {get; private set;}
    public string Nome {get; private set;}
    public string UrlBanco {get; private set;}
    public decimal? ValorLiberado { get; private set; } = null;
    public bool Disponibilidade { get; set; }
    public string MotivoIndisponibilidade { get; set; }
    public BancoItau(int idBanco, string nome, string urlBanco, bool disponibilidade, string motivoIndisponbilidade, decimal? valorLiberado)
    {
        IdBanco = idBanco;
        Nome = nome;
        UrlBanco = urlBanco;
        Disponibilidade = disponibilidade;
        MotivoIndisponibilidade = motivoIndisponbilidade;
        ValorLiberado = valorLiberado.HasValue ? Math.Round(valorLiberado.Value, 2) : null;
    }

    public void CalcularValorLiberado()
    {
        if (ValorLiberado == null)
        {
            Disponibilidade = false;
            MotivoIndisponibilidade = "Valor liberado não informado";
        }

        if (ValorLiberado <= 0)
        {
            Disponibilidade = false;
            MotivoIndisponibilidade = "Valor liberado inválido";
        }

        if (ValorLiberado > 0)
        {
            Disponibilidade = true;
        }
    }
}
