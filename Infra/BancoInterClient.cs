namespace WebApi.Infra;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

public class BancoInterClient
{
    private readonly HttpClient _httpClient;

    public BancoInterClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://cc-api.bancointer.com.br/");
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.DefaultRequestHeaders.Add("Guid", "1b46997b-f587-452b-a177-c4a7bb84bebf");
    }

    public async Task<decimal> SimularAsync(decimal saldoFgts, int qtPeriodos)
    {
        var request = new SimulacaoRequest
        {
            SaldoFgts = saldoFgts,
            QtPeriodos = qtPeriodos
        };

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        var jsonRequest = JsonSerializer.Serialize(request,options);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync("credito-pessoal/simulador-web/v1/simulacoes", content);

        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var simulacaoResponse = JsonSerializer.Deserialize<SimulacaoResponse>(jsonResponse,options);

        if (simulacaoResponse == null)
        {
            throw new Exception("Erro ao serializar resposta da simulação");
        }

        return simulacaoResponse.Valores.ValorLiquido;
    }

    private class SimulacaoRequest
    {
        public decimal SaldoFgts { get; set; }
        public int QtPeriodos { get; set; }
    }

    private class SimulacaoResponse
    {
        public string Status { get; set; }
        public string Guid { get; set; }
        public Valores Valores { get; set; }
    }

    private class Valores
    {
        public int NuParcelas { get; set; }
        public List<Parcela> Parcelas { get; set; }
        public decimal ValorJuros { get; set; }
        public decimal ValorIof { get; set; }
        public decimal ValorLiquido { get; set; }
        public decimal ValorFinanciado { get; set; }
        public decimal ValorTotalParcelas { get; set; }
        public decimal PercentValorLiquido { get; set; }
        public decimal PercentValorJuros { get; set; }
        public decimal PercentValorIof { get; set; }
        public decimal TxJurosEfetivaMensal { get; set; }
        public decimal TxJurosEfetivaAnual { get; set; }
        public decimal TxCetAnual { get; set; }
        public decimal TxCetMensal { get; set; }
    }

    private class Parcela
    {
        public decimal ValorParcela { get; set; }
        public DateTime DataParcela { get; set; }
    }
}