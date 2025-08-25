using Xunit;
using FluentAssertions;
using EcommerceApi.Utils;
using EcommerceApi.DTOs;

namespace EcommerceApi.Tests.Utils
{
    public class SecurityUtilsTests
    {
        [Fact]
        public void HashPassword_ComSenhaValida_DeveRetornarHashNaoVazio()
        {
            // Arrange
            var senha = "123456";

            // Act
            var hash = SecurityUtils.HashPassword(senha);

            // Assert
            hash.Should().NotBeNullOrEmpty();
            hash.Should().NotBe(senha);
        }

        [Fact]
        public void HashPassword_ComSenhasDiferentes_DeveRetornarHashesDiferentes()
        {
            // Arrange
            var senha1 = "123456";
            var senha2 = "654321";

            // Act
            var hash1 = SecurityUtils.HashPassword(senha1);
            var hash2 = SecurityUtils.HashPassword(senha2);

            // Assert
            hash1.Should().NotBe(hash2);
        }

        [Fact]
        public void HashPassword_ComMesmaSenha_DeveRetornarHashConsistente()
        {
            // Arrange
            var senha = "123456";

            // Act
            var hash1 = SecurityUtils.HashPassword(senha);
            var hash2 = SecurityUtils.HashPassword(senha);

            // Assert
            hash1.Should().Be(hash2);
        }

        [Fact]
        public void VerifyPassword_ComSenhaCorreta_DeveRetornarTrue()
        {
            // Arrange
            var senha = "123456";
            var hash = SecurityUtils.HashPassword(senha);

            // Act
            var result = SecurityUtils.VerifyPassword(senha, hash);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void VerifyPassword_ComSenhaIncorreta_DeveRetornarFalse()
        {
            // Arrange
            var senhaCorreta = "123456";
            var senhaIncorreta = "654321";
            var hash = SecurityUtils.HashPassword(senhaCorreta);

            // Act
            var result = SecurityUtils.VerifyPassword(senhaIncorreta, hash);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void VerifyPassword_ComHashVazio_DeveRetornarFalse()
        {
            // Arrange
            var senha = "123456";
            var hash = "";

            // Act
            var result = SecurityUtils.VerifyPassword(senha, hash);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void VerifyPassword_ComSenhaVazia_DeveRetornarFalse()
        {
            // Arrange
            var senha = "";
            var hash = SecurityUtils.HashPassword("123456");

            // Act
            var result = SecurityUtils.VerifyPassword(senha, hash);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void GenerateMockToken_ComUsuarioValido_DeveRetornarTokenNaoVazio()
        {
            // Arrange
            var usuario = new UsuarioResponseDto
            {
                Id = Guid.NewGuid(),
                Nome = "João Silva",
                Email = "joao@teste.com",
                DataCriacao = DateTime.UtcNow
            };

            // Act
            var token = SecurityUtils.GenerateMockToken(usuario);

            // Assert
            token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GenerateMockToken_ComUsuariosDiferentes_DeveRetornarTokensDiferentes()
        {
            // Arrange
            var usuario1 = new UsuarioResponseDto
            {
                Id = Guid.NewGuid(),
                Nome = "João Silva",
                Email = "joao@teste.com",
                DataCriacao = DateTime.UtcNow
            };

            var usuario2 = new UsuarioResponseDto
            {
                Id = Guid.NewGuid(),
                Nome = "Maria Santos",
                Email = "maria@teste.com",
                DataCriacao = DateTime.UtcNow
            };

            // Act
            var token1 = SecurityUtils.GenerateMockToken(usuario1);
            var token2 = SecurityUtils.GenerateMockToken(usuario2);

            // Assert
            token1.Should().NotBe(token2);
        }

        [Fact]
        public void GenerateMockToken_ComMesmoUsuario_DeveRetornarTokensDiferentesDevidoAoTimestamp()
        {
            // Arrange
            var usuario = new UsuarioResponseDto
            {
                Id = Guid.NewGuid(),
                Nome = "João Silva",
                Email = "joao@teste.com",
                DataCriacao = DateTime.UtcNow
            };

            // Act
            var token1 = SecurityUtils.GenerateMockToken(usuario);
            Thread.Sleep(1); // Pequena pausa para garantir timestamp diferente
            var token2 = SecurityUtils.GenerateMockToken(usuario);

            // Assert
            token1.Should().NotBe(token2);
        }

        [Theory]
        [InlineData("")]
        [InlineData("123456")]
        [InlineData("senha_complexa_123!@#")]
        [InlineData("a")]
        public void HashPassword_ComDiferentesSenhas_DeveRetornarHashValido(string senha)
        {
            // Act
            var hash = SecurityUtils.HashPassword(senha);

            // Assert
            hash.Should().NotBeNullOrEmpty();
            hash.Should().NotBe(senha);
            hash.Length.Should().BeGreaterThan(0);
        }

        [Theory]
        [InlineData("123456", "123456", true)]
        [InlineData("123456", "654321", false)]
        [InlineData("senha123", "senha123", true)]
        [InlineData("senha123", "senha456", false)]
        public void VerifyPassword_ComDiferentesSenhas_DeveRetornarResultadoCorreto(string senhaOriginal, string senhaTeste, bool expected)
        {
            // Arrange
            var hash = SecurityUtils.HashPassword(senhaOriginal);

            // Act
            var result = SecurityUtils.VerifyPassword(senhaTeste, hash);

            // Assert
            result.Should().Be(expected);
        }
    }
} 