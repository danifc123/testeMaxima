using EcommerceApi.DTOs;

namespace EcommerceApi.Repositories
{
    public interface IProdutoRepository
    {
        Task<IEnumerable<ProdutoResponseDto>> GetProdutos();
        Task<ProdutoResponseDto?> GetProdutoById(Guid id);
        Task<bool> CodigoExiste(string codigo, Guid? idExcluir = null);
        Task<int> InsertProduto(Guid id, string codigo, string descricao, string departamentoCodigo, decimal preco, bool status);
        Task<int> UpdateProduto(Guid id, string descricao, string departamentoCodigo, decimal preco, bool status);
        Task<int> DeleteProduto(Guid id);
    }
} 