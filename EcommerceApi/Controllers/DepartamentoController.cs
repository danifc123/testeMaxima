using Microsoft.AspNetCore.Mvc;
using EcommerceApi.DTOs;
using EcommerceApi.Constants;

namespace EcommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartamentoController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var departamentos = GetDepartamentosList();
            return Ok(departamentos);
        }

        // Método privado para obter lista de departamentos
        private List<DepartamentoDto> GetDepartamentosList()
        {
            return new List<DepartamentoDto>
            {
                new DepartamentoDto { Codigo = AppConstants.Departamentos.Bebidas, Descricao = "BEBIDAS" },
                new DepartamentoDto { Codigo = AppConstants.Departamentos.Congelados, Descricao = "CONGELADOS" },
                new DepartamentoDto { Codigo = AppConstants.Departamentos.Laticinios, Descricao = "LATICINIOS" },
                new DepartamentoDto { Codigo = AppConstants.Departamentos.Vegetais, Descricao = "VEGETAIS" }
            };
        }
    }
}
