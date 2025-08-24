using EcommerceApi.Constants;
using EcommerceApi.DTOs;

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
    }
} 