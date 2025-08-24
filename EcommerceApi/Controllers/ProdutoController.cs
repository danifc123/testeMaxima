using Microsoft.AspNetCore.Mvc;
using EcommerceApi.Dtos;
using EcommerceApi.Repositories;

namespace EcommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoRepository _repo;

        public ProdutoController(IConfiguration config)
        {
            _repo = new ProdutoRepository(config.GetConnectionString("DefaultConnection")!);
        }

        /// <summary>
        /// Lista todos os produtos ativos
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var produtos = await _repo.GetProdutos();
                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Busca um produto específico por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var produto = await _repo.GetProdutoById(id);
                if (produto == null)
                    return NotFound(new { message = "Produto não encontrado" });

                return Ok(produto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Cria um novo produto
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProdutoDto dto)
        {
            try
            {
                // Validações
                if (string.IsNullOrWhiteSpace(dto.Codigo))
                    return BadRequest(new { message = "Código é obrigatório" });

                if (string.IsNullOrWhiteSpace(dto.Descricao))
                    return BadRequest(new { message = "Descrição é obrigatória" });

                if (dto.Preco <= 0)
                    return BadRequest(new { message = "Preço deve ser maior que zero" });

                // Verifica se o código já existe
                if (await _repo.CodigoExiste(dto.Codigo))
                    return BadRequest(new { message = "Código já existe" });

                var id = Guid.NewGuid();
                await _repo.InsertProduto(id, dto.Codigo, dto.Descricao, dto.DepartamentoCodigo, dto.Preco, dto.Status);
                
                return CreatedAtAction(nameof(GetById), new { id }, new { id, message = "Produto criado com sucesso" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza um produto existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] ProdutoDto dto)
        {
            try
            {
                // Verifica se o produto existe
                var produtoExistente = await _repo.GetProdutoById(id);
                if (produtoExistente == null)
                    return NotFound(new { message = "Produto não encontrado" });

                // Validações
                if (string.IsNullOrWhiteSpace(dto.Descricao))
                    return BadRequest(new { message = "Descrição é obrigatória" });

                if (dto.Preco <= 0)
                    return BadRequest(new { message = "Preço deve ser maior que zero" });

                await _repo.UpdateProduto(id, dto.Descricao, dto.DepartamentoCodigo, dto.Preco, dto.Status);
                return Ok(new { message = "Produto atualizado com sucesso" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Exclui logicamente um produto
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                // Verifica se o produto existe
                var produtoExistente = await _repo.GetProdutoById(id);
                if (produtoExistente == null)
                    return NotFound(new { message = "Produto não encontrado" });

                await _repo.DeleteProduto(id);
                return Ok(new { message = "Produto excluído com sucesso" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", error = ex.Message });
            }
        }
    }
}