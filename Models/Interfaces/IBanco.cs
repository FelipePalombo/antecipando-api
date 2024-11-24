namespace WebApi.Models.Interfaces;

public interface IBanco
{
    int IdBanco { get; }
    string Nome { get; }
    string UrlBanco { get; }
    decimal? ValorLiberado { get; }
    public bool Disponibilidade { get; set; }
    public string MotivoIndisponibilidade { get; set; }
    void CalcularValorLiberado();
}
