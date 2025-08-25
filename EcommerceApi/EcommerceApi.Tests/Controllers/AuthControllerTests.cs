using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using FluentAssertions;
using EcommerceApi.Controllers;
using EcommerceApi.DTOs;
using EcommerceApi.Repositories;
using EcommerceApi.Constants;

namespace EcommerceApi.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IUsuarioRepository> _mockRepository;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockRepository = new Mock<IUsuarioRepository>();
            _controller = new AuthController(_mockRepository.Object);
        }

        [Fact]
        public async Task Register_ComDadosValidos_DeveRetornarCreatedAtAction()
        {
            // Arrange
            var usuarioDto = new UsuarioDto
            {
                Nome = "João Silva",
                Email = "joao@teste.com",
                Senha = "123456"
            };

            _mockRepository.Setup(x => x.EmailExiste(usuarioDto.Email))
                .ReturnsAsync(false);

            _mockRepository.Setup(x => x.InsertUsuario(It.IsAny<Guid>(), usuarioDto.Nome, usuarioDto.Email, It.IsAny<string>()))
                .ReturnsAsync(1);

            // Act
            var result = await _controller.Register(usuarioDto);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
        }

        [Fact]
        public async Task Register_ComNomeVazio_DeveRetornarBadRequest()
        {
            // Arrange
            var usuarioDto = new UsuarioDto
            {
                Nome = "",
                Email = "joao@teste.com",
                Senha = "123456"
            };

            // Act
            var result = await _controller.Register(usuarioDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Register_ComEmailVazio_DeveRetornarBadRequest()
        {
            // Arrange
            var usuarioDto = new UsuarioDto
            {
                Nome = "João Silva",
                Email = "",
                Senha = "123456"
            };

            // Act
            var result = await _controller.Register(usuarioDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Register_ComEmailInvalido_DeveRetornarBadRequest()
        {
            // Arrange
            var usuarioDto = new UsuarioDto
            {
                Nome = "João Silva",
                Email = "emailinvalido",
                Senha = "123456"
            };

            // Act
            var result = await _controller.Register(usuarioDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Register_ComSenhaVazia_DeveRetornarBadRequest()
        {
            // Arrange
            var usuarioDto = new UsuarioDto
            {
                Nome = "João Silva",
                Email = "joao@teste.com",
                Senha = ""
            };

            // Act
            var result = await _controller.Register(usuarioDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Register_ComSenhaCurta_DeveRetornarBadRequest()
        {
            // Arrange
            var usuarioDto = new UsuarioDto
            {
                Nome = "João Silva",
                Email = "joao@teste.com",
                Senha = "123"
            };

            // Act
            var result = await _controller.Register(usuarioDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Register_ComEmailJaExistente_DeveRetornarBadRequest()
        {
            // Arrange
            var usuarioDto = new UsuarioDto
            {
                Nome = "João Silva",
                Email = "joao@teste.com",
                Senha = "123456"
            };

            _mockRepository.Setup(x => x.EmailExiste(usuarioDto.Email))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Register(usuarioDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Login_ComCredenciaisValidas_DeveRetornarOkResult()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "joao@teste.com",
                Senha = "123456"
            };

            var usuario = new UsuarioResponseDto
            {
                Id = Guid.NewGuid(),
                Nome = "João Silva",
                Email = "joao@teste.com",
                DataCriacao = DateTime.UtcNow
            };

            _mockRepository.Setup(x => x.GetUsuarioByEmailAndSenha(loginDto.Email, It.IsAny<string>()))
                .ReturnsAsync(usuario);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Login_ComEmailVazio_DeveRetornarBadRequest()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "",
                Senha = "123456"
            };

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Login_ComSenhaVazia_DeveRetornarBadRequest()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "joao@teste.com",
                Senha = ""
            };

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Login_ComCredenciaisInvalidas_DeveRetornarBadRequest()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "joao@teste.com",
                Senha = "123456"
            };

            _mockRepository.Setup(x => x.GetUsuarioByEmailAndSenha(loginDto.Email, It.IsAny<string>()))
                .ReturnsAsync((UsuarioResponseDto?)null);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetUsuario_ComIdValido_DeveRetornarOkResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var usuario = new UsuarioResponseDto
            {
                Id = id,
                Nome = "João Silva",
                Email = "joao@teste.com",
                DataCriacao = DateTime.UtcNow
            };

            _mockRepository.Setup(x => x.GetUsuarioById(id))
                .ReturnsAsync(usuario);

            // Act
            var result = await _controller.GetUsuario(id);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetUsuario_ComIdInvalido_DeveRetornarNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();

            _mockRepository.Setup(x => x.GetUsuarioById(id))
                .ReturnsAsync((UsuarioResponseDto?)null);

            // Act
            var result = await _controller.GetUsuario(id);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
} 