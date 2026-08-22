using Korp.Faturamento.Dados;
using Microsoft.EntityFrameworkCore;
using Korp.Faturamento.Integracoes.Estoque;
using Korp.Faturamento.Servicos;
using Korp.Faturamento.Middleware;

var construtor = WebApplication.CreateBuilder(args);

construtor.Services.AddControllers();

construtor.Services.AddEndpointsApiExplorer();
construtor.Services.AddSwaggerGen();

construtor.Services.AddDbContext<FaturamentoContexto>(opcoes =>
    opcoes.UseSqlServer(
        construtor.Configuration.GetConnectionString(
            "BancoFaturamento")));

construtor.Services.AddHttpClient<IEstoqueServico, EstoqueServico>(
    cliente =>
    {
        cliente.BaseAddress = new Uri(
            "http://localhost:5174/");
    });

construtor.Services.AddScoped<INotaFiscalServico, NotaFiscalServico>();

construtor.Services.AddCors(opcoes =>
{
    opcoes.AddPolicy("Frontend", politica =>
    {
        politica
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var aplicacao = construtor.Build();



if (aplicacao.Environment.IsDevelopment())
{
    aplicacao.UseSwagger();
    aplicacao.UseSwaggerUI();
}

aplicacao.UseHttpsRedirection();

aplicacao.UseCors("Frontend");

aplicacao.UseMiddleware<TratamentoExcecoesMiddleware>();

aplicacao.MapControllers();

aplicacao.Run();