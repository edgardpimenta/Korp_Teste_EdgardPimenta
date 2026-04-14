namespace ServicoFaturamento.Models
{
    public class NotaFiscal
    {
        public int Id { get; set; }
        public int Numeracao { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public List<ItemNota> Itens { get; set; } = new();
    }
}
