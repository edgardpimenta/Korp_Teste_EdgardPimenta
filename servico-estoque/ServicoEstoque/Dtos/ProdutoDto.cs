namespace ServicoEstoque.Dtos
{
    public class ProdutoDto
    {

    public class ProdutoRequestDto
    {
        public string Codigo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public int Saldo { get; set; }
    }

    public class ProdutoResponseDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public int Saldo { get; set; }
    }

    public class AtualizarSaldoDto
    {
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
    }
}
}
