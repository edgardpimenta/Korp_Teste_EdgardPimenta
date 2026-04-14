namespace ServicoFaturamento.Services;

public class EstoqueService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<EstoqueService> _logger;

    public EstoqueService(HttpClient httpClient, ILogger<EstoqueService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<bool> AtualizarSaldoAsync(int produtoId, int quantidade)
    {
        try
        {
            var payload = new { ProdutoId = produtoId, Quantidade = quantidade };
            var response = await _httpClient.PutAsJsonAsync("api/produtos/atualizar-saldo", payload);

            if (!response.IsSuccessStatusCode)
            {
                var erro = await response.Content.ReadAsStringAsync();
                _logger.LogError("Erro ao atualizar saldo: {Erro}", erro);
                return false;
            }

            return true;
        }
        catch (HttpRequestException ex)
        {
            // Aqui é onde simulamos a falha do microsserviço!
            _logger.LogError("Serviço de Estoque indisponível: {Mensagem}", ex.Message);
            return false;
        }
    }

    public async Task<bool> VerificarDisponibilidadeAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/produtos/health");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}