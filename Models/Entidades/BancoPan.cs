namespace WebApi.Models.Entidades;
using WebApi.Models.Interfaces;

public class BancoPan : IBanco
{
    public int IdBanco {get; private set;}
    public string Nome {get; private set;}
    public string UrlBanco {get; private set;}
    public decimal? ValorLiberado { get; private set; } = null;
    public bool Disponibilidade { get; set; }
    public string MotivoIndisponibilidade { get; set; }
    public BancoPan(int idBanco, string nome, string urlBanco, bool disponibilidade, string motivoIndisponbilidade, decimal? valorLiberado)
    {
        IdBanco = idBanco;
        Nome = nome;
        UrlBanco = urlBanco;
        Disponibilidade = disponibilidade;
        MotivoIndisponibilidade = motivoIndisponbilidade;
        ValorLiberado = valorLiberado;
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
