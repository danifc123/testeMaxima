namespace EcommerceApi.Dtos
{
    public class ProdutoResponseDto
    {
        public Guid Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string DepartamentoCodigo { get; set; } = string.Empty;
        public string DepartamentoDescricao { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public bool Status { get; set; }
        public DateTime DataCriacao { get; set; }
    }
} 