namespace WebApi.Models.Entidades;

using System;

public class Cliente
{
    public DateTime DataNascimento { get; private set; }
    public decimal SaldoFGTS { get; private set; }

    public Cliente(DateTime dataNascimento, decimal saldoFGTS)
    {
        DataNascimento = dataNascimento;
        SaldoFGTS = saldoFGTS;
    }

    public DateTime GetDataNascimento()
    {
        return DataNascimento;
    }

    public decimal GetSaldoFGTS()
    {
        return SaldoFGTS;
    }
}
