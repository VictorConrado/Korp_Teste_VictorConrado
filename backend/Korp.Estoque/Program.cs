using Microsoft.EntityFrameworkCore;
using Korp.Estoque.Dados;
using Korp.Estoque.Servicos;

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

aplicacao.MapControllers();

aplicacao.Run();