namespace WebApi.Infra;

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

public class BancoPagSeguroClient
{
    private readonly HttpClient _httpClient;

    public BancoPagSeguroClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.site.pagseguro.uol.com.br/");
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<string> SimularAsync(decimal saldoFGTS)
    {
        var response = await _httpClient.GetAsync($"ps-website-bff/v1/fgts/simulate?value={saldoFGTS}");

        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var simulacaoResponse = JsonSerializer.Deserialize<SimulacaoResponse>(jsonResponse);

        if (simulacaoResponse == null)
        {
            throw new Exception("Erro ao serializar resposta da simulação");
        }

        return simulacaoResponse.Value;
    }

    private class SimulacaoResponse
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}