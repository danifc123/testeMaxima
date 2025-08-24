using Microsoft.AspNetCore.Mvc;
using EcommerceApi.Dtos;
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

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _repo.GetProdutos());

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProdutoDto dto)
        {
            var id = Guid.NewGuid();
            await _repo.InsertProduto(id, dto.Codigo, dto.Descricao, dto.DepartamentoCodigo, dto.Preco, dto.Status);
            return Ok(new { id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] ProdutoDto dto)
        {
            await _repo.UpdateProduto(id, dto.Descricao, dto.Preco, dto.Status);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _repo.DeleteProduto(id);
            return Ok();
        }
    }
}