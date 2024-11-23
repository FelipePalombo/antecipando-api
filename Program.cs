using System.Text.Json.Serialization;
using WebApi.Helpers;
using WebApi.Services;
using WebApi.Infra;
using WebApi.Models.Entidades;

var builder = WebApplication.CreateBuilder(args);

{
    var services = builder.Services;
    var env = builder.Environment;
 
    services.AddDbContext<DataContext>();
    services.AddCors();
    services.AddControllers().AddJsonOptions(x =>
    {
        // serializar enums como strings nas respostas da API
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        // ignorar parâmetros omitidos nos modelos para habilitar parâmetros opcionais (por exemplo, atualização de usuário)
        x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    // adicionar serviços do Swagger
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Version = "v1",
            Title = "Antecipando API",
            Description = "API para simulação de antecipação de FGTS",
        });
    });

    services.AddHttpClient<BancoInterClient>();
    services.AddHttpClient<BancoPanClient>();
    services.AddHttpClient<BancoPagSeguroClient>();
    services.AddHttpClient<BancoItauClient>();

    // injeção de dependencia
    services.AddScoped<IBancoService, BancoInterService>();
    services.AddScoped<IBancoService, BancoPanService>();
    services.AddScoped<IBancoService, BancoPagSeguroService>();
    services.AddScoped<IBancoService, BancoSantanderService>();
    services.AddScoped<IBancoService, BancoBMGService>();
    services.AddScoped<IBancoService, BancoItauService>();
    services.AddScoped<ISimulacaoService, SimulacaoService>();
}

var app = builder.Build();

// configure o pipeline de requisição HTTP
{
    // global cors policy
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    // manipulador global de erros
    app.UseMiddleware<ErrorHandlerMiddleware>();

    app.MapControllers();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API v1");
        c.RoutePrefix = string.Empty; // Define o Swagger UI na raiz do aplicativo
    });

    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseAuthorization();
}

app.Run("http://localhost:4000");