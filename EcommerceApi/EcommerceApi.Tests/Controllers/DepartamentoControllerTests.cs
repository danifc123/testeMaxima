using Microsoft.AspNetCore.Mvc;
using Xunit;
using FluentAssertions;
using EcommerceApi.Controllers;
using EcommerceApi.DTOs;
using EcommerceApi.Constants;

namespace EcommerceApi.Tests.Controllers
{
    public class DepartamentoControllerTests
    {
        private readonly DepartamentoController _controller;

        public DepartamentoControllerTests()
        {
            _controller = new DepartamentoController();
        }

        [Fact]
        public void Get_DeveRetornarOkResult()
        {
            // Act
            var result = _controller.Get();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void Get_DeveRetornarListaDeDepartamentos()
        {
            // Act
            var result = _controller.Get() as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.Value.Should().BeOfType<List<DepartamentoDto>>();
            
            var departamentos = result.Value as List<DepartamentoDto>;
            departamentos.Should().HaveCount(4);
        }

        [Fact]
        public void Get_DeveConterTodosOsDepartamentosEsperados()
        {
            // Act
            var result = _controller.Get() as OkObjectResult;
            var departamentos = result!.Value as List<DepartamentoDto>;

            // Assert
            departamentos.Should().Contain(d => d.Codigo == AppConstants.Departamentos.Bebidas && d.Descricao == "BEBIDAS");
            departamentos.Should().Contain(d => d.Codigo == AppConstants.Departamentos.Congelados && d.Descricao == "CONGELADOS");
            departamentos.Should().Contain(d => d.Codigo == AppConstants.Departamentos.Laticinios && d.Descricao == "LATICINIOS");
            departamentos.Should().Contain(d => d.Codigo == AppConstants.Departamentos.Vegetais && d.Descricao == "VEGETAIS");
        }

        [Fact]
        public void Get_DeveRetornarDepartamentosComCodigosCorretos()
        {
            // Act
            var result = _controller.Get() as OkObjectResult;
            var departamentos = result!.Value as List<DepartamentoDto>;

            // Assert
            departamentos.Should().Contain(d => d.Codigo == "010");
            departamentos.Should().Contain(d => d.Codigo == "020");
            departamentos.Should().Contain(d => d.Codigo == "030");
            departamentos.Should().Contain(d => d.Codigo == "040");
        }

        [Fact]
        public void Get_DeveRetornarDepartamentosComDescricoesCorretas()
        {
            // Act
            var result = _controller.Get() as OkObjectResult;
            var departamentos = result!.Value as List<DepartamentoDto>;

            // Assert
            departamentos.Should().Contain(d => d.Descricao == "BEBIDAS");
            departamentos.Should().Contain(d => d.Descricao == "CONGELADOS");
            departamentos.Should().Contain(d => d.Descricao == "LATICINIOS");
            departamentos.Should().Contain(d => d.Descricao == "VEGETAIS");
        }

        [Fact]
        public void Get_DeveRetornarDepartamentosSemDuplicatas()
        {
            // Act
            var result = _controller.Get() as OkObjectResult;
            var departamentos = result!.Value as List<DepartamentoDto>;

            // Assert
            departamentos.Should().HaveCount(departamentos.Distinct().Count());
        }

        [Fact]
        public void Get_DeveRetornarDepartamentosComCodigosUnicos()
        {
            // Act
            var result = _controller.Get() as OkObjectResult;
            var departamentos = result!.Value as List<DepartamentoDto>;

            // Assert
            var codigos = departamentos.Select(d => d.Codigo);
            codigos.Should().HaveCount(codigos.Distinct().Count());
        }
    }
} 