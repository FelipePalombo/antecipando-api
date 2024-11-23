namespace WebApi.Infra;

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

public class BancoPanClient
{
    private readonly HttpClient _httpClient;

    public BancoPanClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.bancopan.com.br/");
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<decimal> SimularAsync(decimal saldoDisponivel, DateTime dataAniversario)
    {
        var request = new SimulacaoRequest
        {
            SaldoDisponivel = saldoDisponivel,
            DataAniversario = dataAniversario.ToString("yyyy-MM-dd")
        };

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        var jsonRequest = JsonSerializer.Serialize(request, options);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("institucional/api/simulador/site/emprestimos/fgts/aniversario", content);

        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var simulacaoResponse = JsonSerializer.Deserialize<SimulacaoResponse>(jsonResponse, options);

        if (simulacaoResponse == null)
        {
            throw new Exception("Erro ao serializar resposta da simulação");
        }

        return simulacaoResponse.Results.ValorSaldoDisponivel;
    }

    private class SimulacaoRequest
    {
        [JsonPropertyName("saldo_disponivel")]
        public decimal SaldoDisponivel { get; set; }

        [JsonPropertyName("data_aniversario")]
        public string DataAniversario { get; set; }
    }

    private class SimulacaoResponse
    {
        public Metadata Metadata { get; set; }
        public Results Results { get; set; }
    }

    private class Metadata
    {
        public string Stage { get; set; }
        public string Type { get; set; }
    }

    private class Results
    {
        [JsonPropertyName("valor_saldo_disponivel")]
        public decimal ValorSaldoDisponivel { get; set; }
    }
}