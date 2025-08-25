using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using FluentAssertions;
using EcommerceApi.Controllers;
using EcommerceApi.DTOs;
using EcommerceApi.Constants;
using EcommerceApi.Repositories;

namespace EcommerceApi.Tests.Controllers
{
    public class ProdutoControllerTests
    {
        private readonly Mock<IProdutoRepository> _mockRepository;
        private readonly ProdutoController _controller;

        public ProdutoControllerTests()
        {
            _mockRepository = new Mock<IProdutoRepository>();
            _controller = new ProdutoController(_mockRepository.Object);
        }

        [Fact]
        public async Task Get_DeveRetornarOkResult()
        {
            // Arrange
            var produtos = new List<ProdutoResponseDto>
            {
                new ProdutoResponseDto
                {
                    Id = Guid.NewGuid(),
                    Codigo = "TEST001",
                    Descricao = "Produto Teste",
                    DepartamentoCodigo = AppConstants.Departamentos.Bebidas,
                    Preco = 10.50m,
                    Status = true
                }
            };

            _mockRepository.Setup(x => x.GetProdutos())
                .ReturnsAsync(produtos);

            // Act
            var result = await _controller.Get();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetById_ComIdValido_DeveRetornarOkResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var produto = new ProdutoResponseDto
            {
                Id = id,
                Codigo = "TEST001",
                Descricao = "Produto Teste",
                DepartamentoCodigo = AppConstants.Departamentos.Bebidas,
                Preco = 10.50m,
                Status = true
            };

            _mockRepository.Setup(x => x.GetProdutoById(id))
                .ReturnsAsync(produto);

            // Act
            var result = await _controller.GetById(id);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetById_ComIdInvalido_DeveRetornarNotFound()
        {
            // Arrange
            var id = Guid.Empty;

            // Act
            var result = await _controller.GetById(id);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task Post_ComDadosValidos_DeveRetornarCreatedAtAction()
        {
            // Arrange
            var produtoDto = new ProdutoDto
            {
                Codigo = "TEST001",
                Descricao = "Produto Teste",
                DepartamentoCodigo = AppConstants.Departamentos.Bebidas,
                Preco = 10.50m,
                Status = true
            };

            // Act
            var result = await _controller.Post(produtoDto);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
        }

        [Fact]
        public async Task Post_ComCodigoVazio_DeveRetornarBadRequest()
        {
            // Arrange
            var produtoDto = new ProdutoDto
            {
                Codigo = "",
                Descricao = "Produto Teste",
                DepartamentoCodigo = AppConstants.Departamentos.Bebidas,
                Preco = 10.50m,
                Status = true
            };

            // Act
            var result = await _controller.Post(produtoDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Post_ComDescricaoVazia_DeveRetornarBadRequest()
        {
            // Arrange
            var produtoDto = new ProdutoDto
            {
                Codigo = "TEST001",
                Descricao = "",
                DepartamentoCodigo = AppConstants.Departamentos.Bebidas,
                Preco = 10.50m,
                Status = true
            };

            // Act
            var result = await _controller.Post(produtoDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Post_ComPrecoZero_DeveRetornarBadRequest()
        {
            // Arrange
            var produtoDto = new ProdutoDto
            {
                Codigo = "TEST001",
                Descricao = "Produto Teste",
                DepartamentoCodigo = AppConstants.Departamentos.Bebidas,
                Preco = 0m,
                Status = true
            };

            // Act
            var result = await _controller.Post(produtoDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Post_ComPrecoNegativo_DeveRetornarBadRequest()
        {
            // Arrange
            var produtoDto = new ProdutoDto
            {
                Codigo = "TEST001",
                Descricao = "Produto Teste",
                DepartamentoCodigo = AppConstants.Departamentos.Bebidas,
                Preco = -10.50m,
                Status = true
            };

            // Act
            var result = await _controller.Post(produtoDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Put_ComIdValidoEDadosValidos_DeveRetornarOkResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var produtoDto = new ProdutoDto
            {
                Codigo = "TEST001",
                Descricao = "Produto Atualizado",
                DepartamentoCodigo = AppConstants.Departamentos.Congelados,
                Preco = 15.75m,
                Status = false
            };

            var produtoExistente = new ProdutoResponseDto
            {
                Id = id,
                Codigo = "TEST001",
                Descricao = "Produto Original",
                DepartamentoCodigo = AppConstants.Departamentos.Bebidas,
                Preco = 10.50m,
                Status = true
            };

            _mockRepository.Setup(x => x.GetProdutoById(id))
                .ReturnsAsync(produtoExistente);

            _mockRepository.Setup(x => x.UpdateProduto(id, produtoDto.Descricao, produtoDto.DepartamentoCodigo, produtoDto.Preco, produtoDto.Status))
                .ReturnsAsync(1);

            // Act
            var result = await _controller.Put(id, produtoDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Put_ComIdInvalido_DeveRetornarNotFound()
        {
            // Arrange
            var id = Guid.Empty;
            var produtoDto = new ProdutoDto
            {
                Codigo = "TEST001",
                Descricao = "Produto Atualizado",
                DepartamentoCodigo = AppConstants.Departamentos.Congelados,
                Preco = 15.75m,
                Status = false
            };

            // Act
            var result = await _controller.Put(id, produtoDto);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task Delete_ComIdValido_DeveRetornarOkResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var produto = new ProdutoResponseDto
            {
                Id = id,
                Codigo = "TEST001",
                Descricao = "Produto Teste",
                DepartamentoCodigo = AppConstants.Departamentos.Bebidas,
                Preco = 10.50m,
                Status = true
            };

            _mockRepository.Setup(x => x.GetProdutoById(id))
                .ReturnsAsync(produto);

            _mockRepository.Setup(x => x.DeleteProduto(id))
                .ReturnsAsync(1);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Delete_ComIdInvalido_DeveRetornarNotFound()
        {
            // Arrange
            var id = Guid.Empty;

            // Act
            var result = await _controller.Delete(id);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
} 