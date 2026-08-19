var construtor = WebApplication.CreateBuilder(args);

construtor.Services.AddControllers();

construtor.Services.AddEndpointsApiExplorer();
construtor.Services.AddSwaggerGen();

var aplicacao = construtor.Build();

if (aplicacao.Environment.IsDevelopment())
{
    aplicacao.UseSwagger();
    aplicacao.UseSwaggerUI();
}

aplicacao.UseHttpsRedirection();

aplicacao.MapControllers();

aplicacao.Run();