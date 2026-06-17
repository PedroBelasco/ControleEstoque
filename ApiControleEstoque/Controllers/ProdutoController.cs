using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiControleEstoque.Data;
using ProdutoDomain;
using ApiControleEstoque.Contracts;
using Mapster;

namespace ApiControleEstoque.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProdutoController : ControllerBase
{
    private readonly ApiControleEstoqueContext _context;

    public ProdutoController(ApiControleEstoqueContext context)
    {
        _context = context;
    }

    // GET: api/Produto
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetPesquisaProdutoRequest>>> GetProduto()
    {
        return await _context.Produtos
            .Include(prod => prod.Categoria)
            .Include(prod => prod.UnidadeMedida)
            .Select(prod => new GetPesquisaProdutoRequest
            {
                Id = prod.Id,
                CategoriaNome = prod.Categoria != null ? prod.Categoria.Nome : "",
                ProdutoNome = prod.Nome,
                QuantidadeAtual = prod.QuantidadeAtual,
                UnidadeMedida = prod.UnidadeMedida != null ? prod.UnidadeMedida.Sigla : "",
                NomeArquivoFoto =  prod.NomeArquivoFoto ?? "no-image.png"
            }).ToListAsync();
    }

    // GET: api/Produto/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Produto>> GetProduto(Guid id)
    {
        var produto = await _context.Produtos.FindAsync(id);

        if (produto == null)
        {
            return NotFound();
        }

        return produto;
    }

    [HttpPatch("imagem/{id}")]
    public async Task<IActionResult> PatchProdutoImagem(Guid id, IFormFile arquivo)
    {
        //Busca no banco o produto com o ID passado na url
        var produto = await _context.Produtos.FindAsync(id);

        //Se não encontrar o produto, retornar Not Found
        if (produto == null)
        {
            return NotFound("Produto não encontrado, presta atenção aí");
        }

        //Pega a extensão do arquivo
        var extensao = Path.GetExtension(arquivo.FileName).ToLower();

        //Diretório onde a imagem será colocada
        var diretorioDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagens");

        //Caminho do arquivo + nome do arquivo + extensão do arquivo
        var caminhoArquivo = Path.Combine(diretorioDestino, $"{id}{extensao}");

        //Salva o arquivo conforme o caminho especificado (com nome e extensão definidos)
        //using garante que a variável "stream" seja destruída depois do bloco
        using (var stream = new FileStream(caminhoArquivo, FileMode.Create))
        {
            await arquivo.CopyToAsync(stream);
        }

        var caminhoAntigo = Path.Combine(diretorioDestino, produto.NomeArquivoFoto ?? "");

        if (caminhoArquivo != caminhoAntigo)
        {
            if (System.IO.File.Exists(caminhoAntigo))
            {
                System.IO.File.Delete(caminhoAntigo);
            }
        }

        //Salva no banco de dados o nome do arquivo (formado pelo id do produto + extensao)
        produto.NomeArquivoFoto = Path.Combine($"{id}{extensao}");
        await _context.SaveChangesAsync();

        return NoContent();
    }


    // PUT: api/Produto/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchProduto(Guid id, PatchProdutoRequest request)
    {
        //Busca no banco o produto com o ID passado na url
        var produto = await _context.Produtos.FindAsync(id);

        //Se não encontrar o produto, retornar Not Found
        if (produto == null)
        {
            return NotFound("Produto não encontrado, presta atenção aí");
        }

        //Atualiza o produto conforme o que foi passado no request
        request.Adapt(produto);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProdutoExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Produto
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Produto>> PostProduto(PostProdutoRequest request)
    {
        var produto = new Produto(
            true,
            request.CategoriaId,
            request.UnidadeMedidaId,
            request.Nome,
            request.Descricao,
            request.QuantidadeAtual ?? 0m
            );

        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetProduto", new { id = produto.Id }, produto);
    }

    private bool ProdutoExists(Guid id)
    {
        return _context.Produtos.Any(e => e.Id == id);
    }
}
