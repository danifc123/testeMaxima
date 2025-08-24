using Microsoft.AspNetCore.Mvc;
using EcommerceApi.DTOs;
using EcommerceApi.Repositories;
using EcommerceApi.Constants;
using EcommerceApi.Utils;

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

        // Lista todos os produtos ativos
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
                return HandleInternalError(ex);
            }
        }

        // Busca um produto específico por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var produto = await _repo.GetProdutoById(id);
                if (produto == null)
                    return NotFound(new { message = AppConstants.ValidationMessages.ProdutoNaoEncontrado });

                return Ok(produto);
            }
            catch (Exception ex)
            {
                return HandleInternalError(ex);
            }
        }

        // Cria um novo produto
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProdutoDto dto)
        {
            try
            {
                // Validação usando utilitário
                var (isValid, errorMessage) = ValidationUtils.ValidateProdutoDto(dto);
                if (!isValid)
                    return BadRequest(new { message = errorMessage });

                // Verifica se o código já existe
                if (await _repo.CodigoExiste(dto.Codigo))
                    return BadRequest(new { message = AppConstants.ValidationMessages.CodigoJaExiste });

                var id = Guid.NewGuid();
                await _repo.InsertProduto(id, dto.Codigo, dto.Descricao, dto.DepartamentoCodigo, dto.Preco, dto.Status);
                
                return CreatedAtAction(nameof(GetById), new { id }, new { id, message = AppConstants.SuccessMessages.ProdutoCriado });
            }
            catch (Exception ex)
            {
                return HandleInternalError(ex);
            }
        }

        // Atualiza um produto existente
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] ProdutoDto dto)
        {
            try
            {
                // Verifica se o produto existe
                var produtoExistente = await _repo.GetProdutoById(id);
                if (produtoExistente == null)
                    return NotFound(new { message = AppConstants.ValidationMessages.ProdutoNaoEncontrado });

                // Validação usando utilitário
                var (isValid, errorMessage) = ValidationUtils.ValidateProdutoDto(dto);
                if (!isValid)
                    return BadRequest(new { message = errorMessage });

                await _repo.UpdateProduto(id, dto.Descricao, dto.DepartamentoCodigo, dto.Preco, dto.Status);
                return Ok(new { message = AppConstants.SuccessMessages.ProdutoAtualizado });
            }
            catch (Exception ex)
            {
                return HandleInternalError(ex);
            }
        }

        // Exclui logicamente um produto
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                // Verifica se o produto existe
                var produtoExistente = await _repo.GetProdutoById(id);
                if (produtoExistente == null)
                    return NotFound(new { message = AppConstants.ValidationMessages.ProdutoNaoEncontrado });

                await _repo.DeleteProduto(id);
                return Ok(new { message = AppConstants.SuccessMessages.ProdutoExcluido });
            }
            catch (Exception ex)
            {
                return HandleInternalError(ex);
            }
        }

        // Método privado para tratar erros internos
        private IActionResult HandleInternalError(Exception ex)
        {
            return StatusCode(500, new { message = AppConstants.ErrorMessages.ErroInterno, error = ex.Message });
        }
    }
}