namespace WebApi.Models.Entidades;

using WebApi.Models.Interfaces;
public class Banco : IBanco
{
    public int IdBanco { get; set; }
    public string Nome { get; set; }
    public string UrlBanco { get; set; }
    public decimal? ValorLiberado { get; set; }
    public bool Disponibilidade { get; set; }
    public string MotivoIndisponibilidade { get; set; }

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