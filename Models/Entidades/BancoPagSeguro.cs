namespace WebApi.Models.Entidades;

using System.Globalization;
using WebApi.Models.Interfaces;

public class BancoPagSeguro : IBanco
{
    public int IdBanco {get; private set;}
    public static string Nome {get; private set;} = "Banco PagSeguro";
    public string UrlBanco {get; private set;}
    public decimal? ValorLiberado { get; private set; } = null;
    public bool Disponibilidade { get; set; }
    public string MotivoIndisponbilidade { get; set; }
    public BancoPagSeguro(int idBanco, string urlBanco, bool disponibilidade, string motivoIndisponbilidade, string valorLiberado)
    {
        IdBanco = idBanco;
        UrlBanco = urlBanco;
        Disponibilidade = disponibilidade;
        MotivoIndisponbilidade = motivoIndisponbilidade;
        ValorLiberado = ConverterStringParaDecimal(valorLiberado);
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

    private decimal? ConverterStringParaDecimal(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            return 0;

        valor = valor.Replace("R$", "").Trim();

        var formatInfo = new NumberFormatInfo
        {
            NumberGroupSeparator = ".",
            NumberDecimalSeparator = ","
        };

        return decimal.Parse(valor, formatInfo);
    }
}
