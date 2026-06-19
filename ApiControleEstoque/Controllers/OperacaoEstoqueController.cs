using ApiControleEstoque.Contracts;
using ApiControleEstoque.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OperacaoDomain;

namespace ApiControleEstoque.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OperacaoEstoqueController : ControllerBase
{
    private readonly ApiControleEstoqueContext _context;

    public OperacaoEstoqueController(ApiControleEstoqueContext context)
    {
        _context = context;
    }

    // GET: api/OperacaoEstoque
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OperacaoEstoque>>> GetOperacaoEstoque()
    {
        return await _context.OperacaoEstoque.ToListAsync();
    }

    // GET: api/OperacaoEstoque/5
    [HttpGet("{id}")]
    public async Task<ActionResult<GetOperacaoResponse>> GetOperacaoEstoque(Guid id)
    {
        var resposta = await _context.OperacaoEstoque.
            Where(o => o.Id == id)
            .Select(o => new GetOperacaoResponse
            {
                Id = o.Id,
                Hora = o.Hora,
                EntradaSaida = o.EntradaSaida,
                Motivo = o.Motivo,
                Detalhes = o.Detalhes.Select(detalhe => new OperacaoEstoqueDetalheDTO
                {

                    Id = detalhe.Id,
                    Quantidade = detalhe.Quantidade,
                    NomeProduto = detalhe.Produto != null ? detalhe.Produto.Nome : "",
                    Sigla =
                        detalhe.Produto != null ?
                        detalhe.Produto.UnidadeMedida != null ?
                        detalhe.Produto.UnidadeMedida.Sigla : "" : ""

                }).ToList()
            }).FirstOrDefaultAsync();

        if (resposta == null)
        {
            return NotFound();
        }

        return resposta;

    }



    // PUT: api/OperacaoEstoque/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutOperacaoEstoque(Guid id, OperacaoEstoque operacaoEstoque)
    {
        if (id != operacaoEstoque.Id)
        {
            return BadRequest();
        }

        _context.Entry(operacaoEstoque).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!OperacaoEstoqueExists(id))
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

    // POST: api/OperacaoEstoque
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<OperacaoEstoque>> PostOperacaoEstoque(PostOperacaoRequest request)
    {

        var operacaoEstoque = new OperacaoEstoque
            (
            DateTime.Now,
            request.Motivo,
            request.EntradaSaida
            );

        List<Guid> naoEncontrados = [];

        foreach (var detalhe in request.Detalhes)
        {

            var produto = await _context.Produtos.FindAsync(detalhe.ProdutoId);
            if (produto == null)
            {
                naoEncontrados.Add(detalhe.ProdutoId);
            }
            else
            {
                try
                {
                    operacaoEstoque.CriarDetalhe(produto, detalhe.Quantidade);
                }
                catch (ArgumentException erro)
                {
                    return BadRequest(erro.Message);
                }
            }
        }

        if (naoEncontrados.Count > 0)
        {
            var mensagem = "Produtos nao encontrados: \n";

            foreach (var produtoId in naoEncontrados)
            {
                mensagem += produtoId + "\n";
            }

            return BadRequest(mensagem);
        }

        _context.OperacaoEstoque.Add(operacaoEstoque);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetOperacaoEstoque", new { id = operacaoEstoque.Id }, operacaoEstoque);
    }

    // DELETE: api/OperacaoEstoque/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOperacaoEstoque(Guid id)
    {
        var operacaoEstoque = await _context.OperacaoEstoque.FindAsync(id);
        if (operacaoEstoque == null)
        {
            return NotFound();
        }

        _context.OperacaoEstoque.Remove(operacaoEstoque);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool OperacaoEstoqueExists(Guid id)
    {
        return _context.OperacaoEstoque.Any(e => e.Id == id);
    }
}
