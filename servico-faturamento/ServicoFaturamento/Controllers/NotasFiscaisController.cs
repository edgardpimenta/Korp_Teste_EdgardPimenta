using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServicoFaturamento.Data;
using ServicoFaturamento.DTOs;
using ServicoFaturamento.Models;
using ServicoFaturamento.Services;

namespace ServicoFaturamento.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotasFiscaisController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly EstoqueService _estoqueService;

    public NotasFiscaisController(AppDbContext context, EstoqueService estoqueService)
    {
        _context = context;
        _estoqueService = estoqueService;
    }

    // GET: api/notasfiscais
    [HttpGet]
    public async Task<ActionResult<IEnumerable<NotaFiscalResponseDto>>> GetAll()
    {
        var notas = await _context.NotasFiscais
            .Include(n => n.Itens)
            .OrderByDescending(n => n.Numeracao)
            .Select(n => new NotaFiscalResponseDto
            {
                Id = n.Id,
                Numeracao = n.Numeracao,
                Status = n.Status,
                Itens = n.Itens.Select(i => new ItemNotaResponseDto
                {
                    ProdutoId = i.ProdutoId,
                    Quantidade = i.Quantidade
                }).ToList()
            })
            .ToListAsync();

        return Ok(notas);
    }

    // GET: api/notasfiscais/5
    [HttpGet("{id}")]
    public async Task<ActionResult<NotaFiscalResponseDto>> GetById(int id)
    {
        var nota = await _context.NotasFiscais
            .Include(n => n.Itens)
            .Where(n => n.Id == id)
            .Select(n => new NotaFiscalResponseDto
            {
                Id = n.Id,
                Numeracao = n.Numeracao,
                Status = n.Status,
                Itens = n.Itens.Select(i => new ItemNotaResponseDto
                {
                    ProdutoId = i.ProdutoId,
                    Quantidade = i.Quantidade
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (nota is null)
            return NotFound(new { mensagem = $"Nota fiscal {id} não encontrada." });

        return Ok(nota);
    }

    // POST: api/notasfiscais
    [HttpPost]
    public async Task<ActionResult<NotaFiscalResponseDto>> Create(NotaFiscalRequestDto dto)
    {
        if (!dto.Itens.Any())
            return BadRequest(new { mensagem = "A nota fiscal deve ter pelo menos um item." });

        // LINQ: gera numeração sequencial automática
        var ultimaNumeracao = await _context.NotasFiscais
            .MaxAsync(n => (int?)n.Numeracao) ?? 0;

        var nota = new NotaFiscal
        {
            Numeracao = ultimaNumeracao + 1,
            Status = "Aberta",
            Itens = dto.Itens.Select(i => new ItemNota
            {
                ProdutoId = i.ProdutoId,
                Quantidade = i.Quantidade
            }).ToList()
        };

        _context.NotasFiscais.Add(nota);
        await _context.SaveChangesAsync();

        var response = new NotaFiscalResponseDto
        {
            Id = nota.Id,
            Numeracao = nota.Numeracao,
            Status = nota.Status,
            Itens = nota.Itens.Select(i => new ItemNotaResponseDto
            {
                ProdutoId = i.ProdutoId,
                Quantidade = i.Quantidade
            }).ToList()
        };

        return CreatedAtAction(nameof(GetById), new { id = nota.Id }, response);
    }

    // POST: api/notasfiscais/5/imprimir
    [HttpPost("{id}/imprimir")]
    public async Task<ActionResult> Imprimir(int id)
    {
        var nota = await _context.NotasFiscais
            .Include(n => n.Itens)
            .FirstOrDefaultAsync(n => n.Id == id);

        if (nota is null)
            return NotFound(new { mensagem = $"Nota fiscal {id} não encontrada." });

        if (nota.Status != "Aberta")
            return BadRequest(new { mensagem = "Apenas notas com status Aberta podem ser impressas." });

        // Verifica se o serviço de estoque está disponível
        var estoqueDisponivel = await _estoqueService.VerificarDisponibilidadeAsync();
        if (!estoqueDisponivel)
            return StatusCode(503, new { mensagem = "Serviço de Estoque indisponível. Tente novamente mais tarde." });

        // Atualiza saldo de cada produto
        foreach (var item in nota.Itens)
        {
            var sucesso = await _estoqueService.AtualizarSaldoAsync(item.ProdutoId, item.Quantidade);
            if (!sucesso)
                return StatusCode(503, new { mensagem = $"Erro ao atualizar saldo do produto {item.ProdutoId}." });
        }

        // Fecha a nota
        nota.Status = "Fechada";
        nota.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(new { mensagem = "Nota fiscal impressa com sucesso.", numeracao = nota.Numeracao });
    }

    // GET: api/notasfiscais/health
    [HttpGet("health")]
    public IActionResult Health() => Ok(new { status = "ok", servico = "faturamento" });
}