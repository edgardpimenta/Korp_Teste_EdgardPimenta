using Microsoft.EntityFrameworkCore;
using ServicoFaturamento.Data;
using ServicoFaturamento.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registra o EstoqueService com HttpClient apontando para o Serviço de Estoque
builder.Services.AddHttpClient<EstoqueService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["EstoqueServiceUrl"] ?? "http://localhost:5298/");
    client.Timeout = TimeSpan.FromSeconds(10);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(); 
app.MapScalarApiReference(options =>
{
    options.Title = "Serviço de Faturamento - Korp";
    options.Theme = ScalarTheme.DeepSpace;
});

app.UseCors("AllowAngular");
app.UseAuthorization();
app.MapControllers();

app.Run();