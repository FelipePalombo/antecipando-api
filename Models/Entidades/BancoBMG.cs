namespace WebApi.Models.Entidades;

using System;
using WebApi.Models.Interfaces;

public class BancoBMG : IBanco
{
    public int IdBanco { get; private set; }
    public string Nome { get; private set; }
    public string UrlBanco { get; private set; }
    public decimal? ValorLiberado { get; private set; } = null;
    public bool Disponibilidade { get; set; }
    public string MotivoIndisponibilidade { get; set; }

    private const decimal TAXA_JUROS = 0.0149m; // 1,49%
    private const decimal IOF_DIARIO_PF = 0.000082m; // 0,0082%
    private const decimal IOF_ADICIONAL = 0.0038m; // 0,38%
    private const decimal MAX_ANTECIPADO = 100000m;

    public BancoBMG(int idBanco, string nome, string urlBanco, bool disponibilidade, string motivoIndisponbilidade, decimal? saldoFGTS, DateTime? dataNascimento)
    {
        IdBanco = idBanco;
        Nome = nome;
        UrlBanco = urlBanco;
        if (saldoFGTS == null || dataNascimento == null)
        {
            Disponibilidade = false;
            MotivoIndisponibilidade = motivoIndisponbilidade;
            ValorLiberado = null;
        }
        else
        {
            ValorLiberado = Math.Round(CalcularValorLiberado(saldoFGTS.Value, dataNascimento.Value, 10), 2);
            Disponibilidade = disponibilidade;
        }
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

    private decimal CalcularValorLiberado(decimal saldoFGTS, DateTime dataNascimento, int anos)
    {
        decimal valorParcela, valorDisponivel, valorFinanciado = 0, iof = 0;
        var arrValoresParcelas = new List<decimal>();
        var arrPrazos = new List<int>();

        saldoFGTS = LimitarSaldoAntecipado(saldoFGTS, anos, MAX_ANTECIPADO);
        var dataProposta = DateTime.Now;
        var dataVencimento = CalcularDataVencimento(dataNascimento.Month, dataProposta);

        for (int i = 0; i < anos; i++)
        {
            var prazo = CalcularPrazo(dataProposta, dataVencimento);
            valorParcela = RoundToTwoDecimals(CalcularPmtCaixa(saldoFGTS));
            valorDisponivel = CalcularValorDisponivel(TAXA_JUROS, prazo, valorParcela);

            arrValoresParcelas.Add(valorParcela);
            arrPrazos.Add(prazo);

            valorFinanciado = RoundToTwoDecimals(valorFinanciado + valorDisponivel);
            iof += CalcularIOFParcelado(valorDisponivel, prazo, IOF_DIARIO_PF);

            saldoFGTS = RoundToTwoDecimals(saldoFGTS - valorParcela);
            dataVencimento = dataVencimento.AddYears(1);
        }

        iof = RoundToTwoDecimals(iof + valorFinanciado * IOF_ADICIONAL);
        var valorLiberado = RoundToTwoDecimals(valorFinanciado - iof);

        if (valorLiberado > MAX_ANTECIPADO)
        {
            throw new Exception("O valor total do saque aniversário não pode ser maior que R$ 100.000,00.");
        }

        return valorLiberado;
    }

    private decimal CalcularValorDisponivel(decimal taxa, int periodo, decimal valorFuturo)
    {
        var valorPresente = valorFuturo / (decimal)Math.Pow((double)(1 + taxa), periodo / 30.0);
        return Math.Round(valorPresente, 2);
    }

    private decimal CalcularIOFParcelado(decimal valorDisponivel, int prazo, decimal iofDiarioPF)
    {
        prazo = prazo > 365 ? 365 : prazo;
        return Math.Round(valorDisponivel * iofDiarioPF * prazo, 2);
    }

    private decimal RoundToTwoDecimals(decimal number)
    {
        return Math.Round(number, 2);
    }

    private int CalcularPrazo(DateTime dataInicial, DateTime dataFinal)
    {
        return (int)(dataFinal - dataInicial).TotalDays;
    }

    private DateTime CalcularDataVencimento(int mesAniversario, DateTime dataReferencial)
    {
        var vencimento = new DateTime(dataReferencial.Year, mesAniversario, 1);
        if (mesAniversario < dataReferencial.Month)
        {
            vencimento = vencimento.AddYears(1);
        }
        return vencimento;
    }

    private decimal LimitarSaldoAntecipado(decimal saldo, int qtdParcelas, decimal maxValue)
    {
        decimal valorAntecipacao, saldoParcial, valorParcela;
        decimal novoSaldo = saldo > (maxValue * 3.5m) ? (maxValue * 3.5m) : saldo;

        novoSaldo = RoundToTwoDecimals(novoSaldo + 0.01m);
        do
        {
            valorAntecipacao = 0;
            novoSaldo = RoundToTwoDecimals(novoSaldo - 0.01m);
            saldoParcial = novoSaldo;

            for (int i = 0; i < qtdParcelas; i++)
            {
                valorParcela = RoundToTwoDecimals(CalcularPmtCaixa(saldoParcial));
                valorAntecipacao = RoundToTwoDecimals(valorAntecipacao + valorParcela);
                if (valorAntecipacao > maxValue) break;
                saldoParcial = RoundToTwoDecimals(saldoParcial - valorParcela);
            }
        } while (valorAntecipacao > maxValue);

        return novoSaldo;
    }

    private decimal CalcularPmtCaixa(decimal saldo)
    {
        if (saldo > 20000) return saldo * 0.05m + 2900;
        if (saldo > 15000) return saldo * 0.1m + 1900;
        if (saldo > 10000) return saldo * 0.15m + 1150;
        if (saldo > 5000) return saldo * 0.2m + 650;
        if (saldo > 1000) return saldo * 0.3m + 150;
        if (saldo > 500) return saldo * 0.4m + 50;

        return saldo * 0.5m;
    }
}