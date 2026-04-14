namespace ServicoFaturamento.DTOs;

public class ItemNotaRequestDto
{
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
}

public class NotaFiscalRequestDto
{
    public List<ItemNotaRequestDto> Itens { get; set; } = new();
}

public class ItemNotaResponseDto
{
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
}

public class NotaFiscalResponseDto
{
    public int Id { get; set; }
    public int Numeracao { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<ItemNotaResponseDto> Itens { get; set; } = new();
}