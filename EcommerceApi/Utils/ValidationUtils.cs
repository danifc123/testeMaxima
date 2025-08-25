using EcommerceApi.Constants;
using EcommerceApi.DTOs;
using System.Text.RegularExpressions;

namespace EcommerceApi.Utils
{
    public static class ValidationUtils
    {
        // Valida se um produto DTO é válido
        public static (bool isValid, string errorMessage) ValidateProdutoDto(ProdutoDto produto)
        {
            if (string.IsNullOrWhiteSpace(produto.Codigo))
                return (false, AppConstants.ValidationMessages.CampoObrigatorio);

            if (produto.Codigo.Length > AppConstants.Validation.CodigoMaxLength)
                return (false, $"Código deve ter no máximo {AppConstants.Validation.CodigoMaxLength} caracteres");

            if (string.IsNullOrWhiteSpace(produto.Descricao))
                return (false, AppConstants.ValidationMessages.CampoObrigatorio);

            if (produto.Descricao.Length > AppConstants.Validation.DescricaoMaxLength)
                return (false, $"Descrição deve ter no máximo {AppConstants.Validation.DescricaoMaxLength} caracteres");

            if (string.IsNullOrWhiteSpace(produto.DepartamentoCodigo))
                return (false, AppConstants.ValidationMessages.CampoObrigatorio);

            if (produto.Preco < AppConstants.Validation.PrecoMinimo)
                return (false, AppConstants.ValidationMessages.PrecoMinimo);

            return (true, string.Empty);
        }

        // Valida se um ID é um GUID válido
        public static bool IsValidGuid(string id)
        {
            return Guid.TryParse(id, out _);
        }

        // Valida se um usuário DTO é válido
        public static (bool isValid, string errorMessage) ValidateUsuarioDto(UsuarioDto usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.Nome))
                return (false, AppConstants.ValidationMessages.NomeObrigatorio);

            if (string.IsNullOrWhiteSpace(usuario.Email))
                return (false, AppConstants.ValidationMessages.EmailObrigatorio);

            if (!IsValidEmail(usuario.Email))
                return (false, AppConstants.ValidationMessages.EmailInvalido);

            if (string.IsNullOrWhiteSpace(usuario.Senha))
                return (false, AppConstants.ValidationMessages.SenhaObrigatoria);

            if (usuario.Senha.Length < AppConstants.Validation.MinLengthSenha)
                return (false, AppConstants.ValidationMessages.SenhaMinima);

            return (true, string.Empty);
        }

        // Valida se um login DTO é válido
        public static (bool isValid, string errorMessage) ValidateLoginDto(LoginDto login)
        {
            if (string.IsNullOrWhiteSpace(login.Email))
                return (false, AppConstants.ValidationMessages.EmailObrigatorio);

            if (string.IsNullOrWhiteSpace(login.Senha))
                return (false, AppConstants.ValidationMessages.SenhaObrigatoria);

            return (true, string.Empty);
        }

        // Valida se um email é válido
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }
    }
} 