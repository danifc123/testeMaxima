namespace EcommerceApi.DTOs
{
    public class ProdutoDto
    {
        public string Codigo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string DepartamentoCodigo { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public bool Status { get; set; }
    }
}
