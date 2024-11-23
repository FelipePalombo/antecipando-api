namespace WebApi.Models.Entidades;

using System;

public class Cliente
{
    public int IdUsuario { get; private set; }
    public DateTime DataNascimento { get; private set; }
    public decimal SaldoFGTS { get; private set; }

    public Cliente(DateTime dataNascimento, decimal saldoFGTS)
    {
        IdUsuario = new Random().Next(1, int.MaxValue);
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
