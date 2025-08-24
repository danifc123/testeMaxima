using Microsoft.AspNetCore.Mvc;
using EcommerceApi.Dtos;

namespace EcommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartamentoController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var departamentos = new List<DepartamentoDto>
            {
                new DepartamentoDto { Codigo = "010", Descricao = "BEBIDAS" },
                new DepartamentoDto { Codigo = "020", Descricao = "CONGELADOS" },
                new DepartamentoDto { Codigo = "030", Descricao = "LATICINIOS" },
                new DepartamentoDto { Codigo = "040", Descricao = "VEGETAIS" }
            };

            return Ok(departamentos);
        }
    }
}
