using Korp.Estoque.Dados;
using Korp.Estoque.Middleware;
using Korp.Estoque.Servicos;
using Microsoft.EntityFrameworkCore;


var construtor = WebApplication.CreateBuilder(args);

construtor.Services.AddControllers();

construtor.Services.AddEndpointsApiExplorer();
construtor.Services.AddSwaggerGen();

construtor.Services.AddDbContext<EstoqueContexto>(opcoes =>
    opcoes.UseSqlServer(
        construtor.Configuration.GetConnectionString("BancoEstoque")));

construtor.Services.AddScoped<IProdutoServico, ProdutoServico>();

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

aplicacao.UseCors("Frontend");

if (aplicacao.Environment.IsDevelopment())
{
    aplicacao.UseSwagger();
    aplicacao.UseSwaggerUI();
}

aplicacao.UseHttpsRedirection();

aplicacao.UseMiddleware<TratamentoExcecoesMiddleware>();

aplicacao.MapControllers();

aplicacao.Run();