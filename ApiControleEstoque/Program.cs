using Microsoft.EntityFrameworkCore;
using ApiControleEstoque.Data;
using Mapster;
using ApiControleEstoque.Contracts;
using ProdutoDomain;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApiControleEstoqueContext>(options =>
    options.UseSqlServer(
        builder.Configuration
        .GetConnectionString("ApiControleEstoqueContext") ?? 
        throw new InvalidOperationException("Connection string 'ApiControleEstoqueContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

TypeAdapterConfig<PatchProdutoRequest, Produto>.NewConfig().IgnoreNullValues(true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors(politica => politica.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles();

app.Run();
