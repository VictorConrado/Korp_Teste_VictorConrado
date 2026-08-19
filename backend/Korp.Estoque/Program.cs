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

var aplicacao = construtor.Build();

if (aplicacao.Environment.IsDevelopment())
{
    aplicacao.UseSwagger();
    aplicacao.UseSwaggerUI();
}

aplicacao.UseHttpsRedirection();

aplicacao.UseMiddleware<TratamentoExcecoesMiddleware>();

aplicacao.MapControllers();

aplicacao.Run();