namespace WebApi.Models.DTOs;

public class ResultadoSimulacaoPreDefinidaDTO
{
    public int IdSolicitacao { get; set; }
    public DateTime DataSolicitacao { get; set; }
    public DateTime DataNascimento { get; set; }
    public decimal SaldoFGTS { get; set; }
    public int IdBanco { get; set; }
    public decimal? ValorLiberado { get; set; }
    public bool Disponibilidade { get; set; }
    public string MotivoIndisponibilidade { get; set; }
    public string Nome { get; set; }
    public string UrlBanco { get; set; }
}