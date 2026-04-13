using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServicoEstoque.Data;
using ServicoEstoque.Models;
using static ServicoEstoque.Dtos.ProdutoDto;

namespace ServicoEstoque.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProdutosController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/produtos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProdutoResponseDto>>> GetAll()
    {
        var produtos = await _context.Produtos
            .OrderBy(p => p.Descricao)
            .Select(p => new ProdutoResponseDto
            {
                Id = p.Id,
                Codigo = p.Codigo,
                Descricao = p.Descricao,
                Saldo = p.Saldo
            })
            .ToListAsync();

        return Ok(produtos);
    }

    // GET: api/produtos/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ProdutoResponseDto>> GetById(int id)
    {
        var produto = await _context.Produtos
            .Where(p => p.Id == id)
            .Select(p => new ProdutoResponseDto
            {
                Id = p.Id,
                Codigo = p.Codigo,
                Descricao = p.Descricao,
                Saldo = p.Saldo
            })
            .FirstOrDefaultAsync();

        if (produto is null)
            return NotFound(new { mensagem = $"Produto {id} não encontrado." });

        return Ok(produto);
    }

    // POST: api/produtos
    [HttpPost]
    public async Task<ActionResult<ProdutoResponseDto>> Create(ProdutoRequestDto dto)
    {
        // LINQ: verifica se código já existe
        var existe = await _context.Produtos
            .AnyAsync(p => p.Codigo == dto.Codigo);

        if (existe)
            return Conflict(new { mensagem = $"Já existe um produto com o código '{dto.Codigo}'." });

        var produto = new Produto
        {
            Codigo = dto.Codigo,
            Descricao = dto.Descricao,
            Saldo = dto.Saldo
        };

        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();

        var response = new ProdutoResponseDto
        {
            Id = produto.Id,
            Codigo = produto.Codigo,
            Descricao = produto.Descricao,
            Saldo = produto.Saldo
        };

        return CreatedAtAction(nameof(GetById), new { id = produto.Id }, response);
    }

    // PUT: api/produtos/5
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, ProdutoRequestDto dto)
    {
        var produto = await _context.Produtos.FindAsync(id);

        if (produto is null)
            return NotFound(new { mensagem = $"Produto {id} não encontrado." });

        // LINQ: verifica se outro produto já usa esse código
        var codigoDuplicado = await _context.Produtos
            .AnyAsync(p => p.Codigo == dto.Codigo && p.Id != id);

        if (codigoDuplicado)
            return Conflict(new { mensagem = $"Já existe outro produto com o código '{dto.Codigo}'." });

        produto.Codigo = dto.Codigo;
        produto.Descricao = dto.Descricao;
        produto.Saldo = dto.Saldo;
        produto.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/produtos/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var produto = await _context.Produtos.FindAsync(id);

        if (produto is null)
            return NotFound(new { mensagem = $"Produto {id} não encontrado." });

        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // PUT: api/produtos/atualizar-saldo
    // Usado pelo Serviço de Faturamento na impressão da nota
    [HttpPut("atualizar-saldo")]
    public async Task<ActionResult> AtualizarSaldo(AtualizarSaldoDto dto)
    {
        var produto = await _context.Produtos.FindAsync(dto.ProdutoId);

        if (produto is null)
            return NotFound(new { mensagem = $"Produto {dto.ProdutoId} não encontrado." });

        if (produto.Saldo < dto.Quantidade)
            return BadRequest(new { mensagem = $"Saldo insuficiente. Saldo atual: {produto.Saldo}" });

        produto.Saldo -= dto.Quantidade;
        produto.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // GET: api/produtos/health
    [HttpGet("health")]
    public IActionResult Health() => Ok(new { status = "ok", servico = "estoque" });
}