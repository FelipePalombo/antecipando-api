namespace WebApi.Infra;

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

public class BancoItauClient
{
    private readonly HttpClient _httpClient;
    private string _token;

    public BancoItauClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.emprestimo.itau.com.br/");
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    private async Task<string> ObterTokenAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "v2/authorization/");
        request.Headers.Add("App", "8ec6184a-0d03-497d-bcd5-dcf911f2d479");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(jsonResponse);

        if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.Token))
        {
            throw new Exception("Erro ao obter o token do Banco Itau");
        }

        _token = tokenResponse.Token;
        return _token;
    }

    public async Task<decimal> SimularAsync(decimal saldoDisponivel, DateTime dataAniversario)
    {
        if (string.IsNullOrEmpty(_token))
        {
            await ObterTokenAsync();
        }

        var request = new SimulacaoRequest
        {
            FgtsBalance = saldoDisponivel,
            Birthdate = dataAniversario.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
        };

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        var jsonRequest = JsonSerializer.Serialize(request, options);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        var simulationRequest = new HttpRequestMessage(HttpMethod.Post, "v2/itau-loans-api/fgts/simulation/?partner=false")
        {
            Content = content
        };

        var tokenHeader = "Bearer " + _token;
        
        simulationRequest.Headers.Add("Token", tokenHeader);

        var response = await _httpClient.SendAsync(simulationRequest);
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var simulacaoResponse = JsonSerializer.Deserialize<SimulacaoResponse>(jsonResponse, options);

        if (simulacaoResponse == null)
        {
            throw new Exception("Erro ao serializar resposta da simulação");
        }

        return simulacaoResponse.AvailableValue;
    }

    private class TokenResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("valid_time")]
        public long ValidTime { get; set; }

        [JsonPropertyName("server_time")]
        public long ServerTime { get; set; }
    }

    private class SimulacaoRequest
    {
        [JsonPropertyName("fgts_balance")]
        public decimal FgtsBalance { get; set; }

        [JsonPropertyName("birthdate")]
        public string Birthdate { get; set; }
    }

    private class SimulacaoResponse
    {
        [JsonPropertyName("available_value")]
        public decimal AvailableValue { get; set; }

        [JsonPropertyName("interest_rate")]
        public decimal InterestRate { get; set; }
    }
}