namespace WebApi.Models.Entidades;

using WebApi.Models.Interfaces;
using System;

public class BancoSantander : IBanco
{
    public int IdBanco { get; private set; }
    public static string Nome { get; private set; } = "Banco Santander";
    public string UrlBanco { get; private set; }
    public decimal? ValorLiberado { get; private set; } = null;
    public bool Disponibilidade { get; set; }
    public string MotivoIndisponbilidade { get; set; }

    public BancoSantander(int idBanco, string urlBanco, bool disponibilidade, string motivoIndisponbilidade, decimal? saldoFGTS, DateTime? dataNascimento)
    {
        IdBanco = idBanco;
        UrlBanco = urlBanco;
        if (saldoFGTS == null || dataNascimento == null)
        {
            Disponibilidade = false;
            MotivoIndisponbilidade = motivoIndisponbilidade;
            ValorLiberado = null;
        }
        else
        {
            ValorLiberado = Math.Round(CalcularValorLiberado(saldoFGTS.Value, dataNascimento.Value), 2);
            Disponibilidade = disponibilidade;
        }
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

    private decimal CalcularValorLiberado(decimal saldoFGTS, DateTime dataNascimento)
    {
        var resultados = new decimal[5];
        for (int i = 0; i < 5; i++)
        {
            resultados[i] = CalcularResultado(saldoFGTS, i);
        }

        var totalSaqueAniversario = 0m;
        foreach (var resultado in resultados)
        {
            totalSaqueAniversario += resultado;
        }

        if (totalSaqueAniversario > 50000)
        {
            throw new Exception("O valor total do saque aniversário não pode ser maior que R$ 50.000,00.");
        }

        return totalSaqueAniversario;
    }

    private decimal CalcularResultado(decimal valorFGTS, int ano)
    {
        var resultados = new decimal[] { 0.5m, 0.4m, 0.3m, 0.2m, 0.15m, 0.1m, 0.05m };
        var limites = new decimal[] { 500m, 1000m, 5000m, 10000m, 15000m, 20000m };
        var adicoes = new decimal[] { 0m, 50m, 150m, 650m, 1150m, 1900m, 2900m };

        var abatimento = valorFGTS;
        for (int i = 0; i < ano; i++)
        {
            abatimento -= CalcularResultadoAno(abatimento, resultados, limites, adicoes);
        }

        return CalcularResultadoAno(abatimento, resultados, limites, adicoes);
    }

    private decimal CalcularResultadoAno(decimal valor, decimal[] resultados, decimal[] limites, decimal[] adicoes)
    {
        for (int i = 0; i < limites.Length; i++)
        {
            if (valor <= limites[i])
            {
                return valor * resultados[i] + adicoes[i];
            }
        }
        return valor * resultados[6] + adicoes[6];
    }
}
