using EcommerceApi.DTOs;

namespace EcommerceApi.Repositories
{
    public interface IUsuarioRepository
    {
        Task<UsuarioResponseDto?> GetUsuarioByEmail(string email);
        Task<UsuarioResponseDto?> GetUsuarioById(Guid id);
        Task<bool> EmailExiste(string email);
        Task<UsuarioResponseDto?> GetUsuarioByEmailAndSenha(string email, string senhaHash);
        Task<int> InsertUsuario(Guid id, string nome, string email, string senhaHash);
    }
} 