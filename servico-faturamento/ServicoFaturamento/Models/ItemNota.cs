namespace ServicoFaturamento.Models
{
    public class ItemNota
    {
        public int Id { get; set; }
        public int NotaFiscalId { get; set; }
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public NotaFiscal NotaFiscal { get; set; } = null!;
    }
}
