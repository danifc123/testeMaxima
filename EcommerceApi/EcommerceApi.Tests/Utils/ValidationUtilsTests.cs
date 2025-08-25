using Xunit;
using FluentAssertions;
using EcommerceApi.Utils;
using EcommerceApi.DTOs;
using EcommerceApi.Constants;

namespace EcommerceApi.Tests.Utils
{
    public class ValidationUtilsTests
    {
        [Fact]
        public void ValidateProdutoDto_ComDadosValidos_DeveRetornarTrue()
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
            var (isValid, errorMessage) = ValidationUtils.ValidateProdutoDto(produtoDto);

            // Assert
            isValid.Should().BeTrue();
            errorMessage.Should().BeEmpty();
        }

        [Fact]
        public void ValidateProdutoDto_ComCodigoVazio_DeveRetornarFalse()
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
            var (isValid, errorMessage) = ValidationUtils.ValidateProdutoDto(produtoDto);

            // Assert
            isValid.Should().BeFalse();
            errorMessage.Should().Be(AppConstants.ValidationMessages.CampoObrigatorio);
        }

        [Fact]
        public void ValidateProdutoDto_ComDescricaoVazia_DeveRetornarFalse()
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
            var (isValid, errorMessage) = ValidationUtils.ValidateProdutoDto(produtoDto);

            // Assert
            isValid.Should().BeFalse();
            errorMessage.Should().Be(AppConstants.ValidationMessages.CampoObrigatorio);
        }

        [Fact]
        public void ValidateProdutoDto_ComPrecoZero_DeveRetornarFalse()
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
            var (isValid, errorMessage) = ValidationUtils.ValidateProdutoDto(produtoDto);

            // Assert
            isValid.Should().BeFalse();
            errorMessage.Should().Be(AppConstants.ValidationMessages.PrecoMinimo);
        }

        [Fact]
        public void ValidateProdutoDto_ComPrecoNegativo_DeveRetornarFalse()
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
            var (isValid, errorMessage) = ValidationUtils.ValidateProdutoDto(produtoDto);

            // Assert
            isValid.Should().BeFalse();
            errorMessage.Should().Be(AppConstants.ValidationMessages.PrecoMinimo);
        }

        [Fact]
        public void ValidateUsuarioDto_ComDadosValidos_DeveRetornarTrue()
        {
            // Arrange
            var usuarioDto = new UsuarioDto
            {
                Nome = "João Silva",
                Email = "joao@teste.com",
                Senha = "123456"
            };

            // Act
            var (isValid, errorMessage) = ValidationUtils.ValidateUsuarioDto(usuarioDto);

            // Assert
            isValid.Should().BeTrue();
            errorMessage.Should().BeEmpty();
        }

        [Fact]
        public void ValidateUsuarioDto_ComNomeVazio_DeveRetornarFalse()
        {
            // Arrange
            var usuarioDto = new UsuarioDto
            {
                Nome = "",
                Email = "joao@teste.com",
                Senha = "123456"
            };

            // Act
            var (isValid, errorMessage) = ValidationUtils.ValidateUsuarioDto(usuarioDto);

            // Assert
            isValid.Should().BeFalse();
            errorMessage.Should().Be(AppConstants.ValidationMessages.NomeObrigatorio);
        }

        [Fact]
        public void ValidateUsuarioDto_ComEmailVazio_DeveRetornarFalse()
        {
            // Arrange
            var usuarioDto = new UsuarioDto
            {
                Nome = "João Silva",
                Email = "",
                Senha = "123456"
            };

            // Act
            var (isValid, errorMessage) = ValidationUtils.ValidateUsuarioDto(usuarioDto);

            // Assert
            isValid.Should().BeFalse();
            errorMessage.Should().Be(AppConstants.ValidationMessages.EmailObrigatorio);
        }

        [Fact]
        public void ValidateUsuarioDto_ComEmailInvalido_DeveRetornarFalse()
        {
            // Arrange
            var usuarioDto = new UsuarioDto
            {
                Nome = "João Silva",
                Email = "emailinvalido",
                Senha = "123456"
            };

            // Act
            var (isValid, errorMessage) = ValidationUtils.ValidateUsuarioDto(usuarioDto);

            // Assert
            isValid.Should().BeFalse();
            errorMessage.Should().Be(AppConstants.ValidationMessages.EmailInvalido);
        }

        [Fact]
        public void ValidateUsuarioDto_ComSenhaVazia_DeveRetornarFalse()
        {
            // Arrange
            var usuarioDto = new UsuarioDto
            {
                Nome = "João Silva",
                Email = "joao@teste.com",
                Senha = ""
            };

            // Act
            var (isValid, errorMessage) = ValidationUtils.ValidateUsuarioDto(usuarioDto);

            // Assert
            isValid.Should().BeFalse();
            errorMessage.Should().Be(AppConstants.ValidationMessages.SenhaObrigatoria);
        }

        [Fact]
        public void ValidateUsuarioDto_ComSenhaCurta_DeveRetornarFalse()
        {
            // Arrange
            var usuarioDto = new UsuarioDto
            {
                Nome = "João Silva",
                Email = "joao@teste.com",
                Senha = "123"
            };

            // Act
            var (isValid, errorMessage) = ValidationUtils.ValidateUsuarioDto(usuarioDto);

            // Assert
            isValid.Should().BeFalse();
            errorMessage.Should().Be(AppConstants.ValidationMessages.SenhaMinima);
        }

        [Fact]
        public void ValidateLoginDto_ComDadosValidos_DeveRetornarTrue()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "joao@teste.com",
                Senha = "123456"
            };

            // Act
            var (isValid, errorMessage) = ValidationUtils.ValidateLoginDto(loginDto);

            // Assert
            isValid.Should().BeTrue();
            errorMessage.Should().BeEmpty();
        }

        [Fact]
        public void ValidateLoginDto_ComEmailVazio_DeveRetornarFalse()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "",
                Senha = "123456"
            };

            // Act
            var (isValid, errorMessage) = ValidationUtils.ValidateLoginDto(loginDto);

            // Assert
            isValid.Should().BeFalse();
            errorMessage.Should().Be(AppConstants.ValidationMessages.EmailObrigatorio);
        }

        [Fact]
        public void ValidateLoginDto_ComSenhaVazia_DeveRetornarFalse()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "joao@teste.com",
                Senha = ""
            };

            // Act
            var (isValid, errorMessage) = ValidationUtils.ValidateLoginDto(loginDto);

            // Assert
            isValid.Should().BeFalse();
            errorMessage.Should().Be(AppConstants.ValidationMessages.SenhaObrigatoria);
        }

        [Theory]
        [InlineData("teste@email.com", true)]
        [InlineData("usuario@dominio.com.br", true)]
        [InlineData("a@b.c", true)]
        [InlineData("emailinvalido", false)]
        [InlineData("@email.com", false)]
        [InlineData("email@", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void IsValidEmail_ComDiferentesEmails_DeveRetornarResultadoCorreto(string email, bool expected)
        {
            // Act
            var result = ValidationUtils.IsValidEmail(email);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("123e4567-e89b-12d3-a456-426614174000", true)]
        [InlineData("00000000-0000-0000-0000-000000000000", true)]
        [InlineData("invalid-guid", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void IsValidGuid_ComDiferentesValores_DeveRetornarResultadoCorreto(string id, bool expected)
        {
            // Act
            var result = ValidationUtils.IsValidGuid(id);

            // Assert
            result.Should().Be(expected);
        }
    }
} 