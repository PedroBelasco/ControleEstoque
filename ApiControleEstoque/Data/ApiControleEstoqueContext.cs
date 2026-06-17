using Microsoft.EntityFrameworkCore;

using ProdutoDomain;

namespace ApiControleEstoque.Data;

public class ApiControleEstoqueContext : DbContext
{
    public ApiControleEstoqueContext(DbContextOptions<ApiControleEstoqueContext> options)
        : base(options)
    {
    }

    public DbSet<UnidadeMedida> UnidadesMedida { get; set; } = default!;
    public DbSet<CategoriaProduto> CategoriasProduto { get; set; } = default!;
    public DbSet<Produto> Produtos { get; set; } = default!;
}
