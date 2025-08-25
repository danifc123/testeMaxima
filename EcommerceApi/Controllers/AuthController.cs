using Microsoft.AspNetCore.Mvc;
using EcommerceApi.DTOs;
using EcommerceApi.Repositories;
using EcommerceApi.Utils;
using EcommerceApi.Constants;

namespace EcommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioRepository _repo;

        public AuthController(IUsuarioRepository repo)
        {
            _repo = repo;
        }

        // Registra um novo usuário
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UsuarioDto dto)
        {
            try
            {
                // Validação usando utilitário
                var (isValid, errorMessage) = ValidationUtils.ValidateUsuarioDto(dto);
                if (!isValid)
                    return BadRequest(new { message = errorMessage });

                // Verifica se o email já existe
                if (await _repo.EmailExiste(dto.Email))
                    return BadRequest(new { message = AppConstants.ValidationMessages.EmailJaExiste });

                // Criptografa a senha
                var senhaHash = SecurityUtils.HashPassword(dto.Senha);

                // Cria o usuário
                var id = Guid.NewGuid();
                await _repo.InsertUsuario(id, dto.Nome, dto.Email, senhaHash);

                // Retorna o usuário criado (sem senha)
                var usuario = new UsuarioResponseDto
                {
                    Id = id,
                    Nome = dto.Nome,
                    Email = dto.Email,
                    DataCriacao = DateTime.UtcNow
                };

                return CreatedAtAction(nameof(GetUsuario), new { id }, new { 
                    usuario, 
                    message = AppConstants.SuccessMessages.UsuarioCriado 
                });
            }
            catch (Exception ex)
            {
                return HandleInternalError(ex);
            }
        }

        // Realiza login do usuário
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                // Validação usando utilitário
                var (isValid, errorMessage) = ValidationUtils.ValidateLoginDto(dto);
                if (!isValid)
                    return BadRequest(new { message = errorMessage });

                // Criptografa a senha para comparação
                var senhaHash = SecurityUtils.HashPassword(dto.Senha);

                // Busca o usuário pelo email e senha
                var usuario = await _repo.GetUsuarioByEmailAndSenha(dto.Email, senhaHash);
                if (usuario == null)
                    return BadRequest(new { message = AppConstants.ValidationMessages.CredenciaisInvalidas });

                // Gera o token (mock para desenvolvimento)
                var token = SecurityUtils.GenerateMockToken(usuario);

                var response = new LoginResponseDto
                {
                    Token = token,
                    Usuario = usuario
                };

                return Ok(new { 
                    data = response, 
                    message = AppConstants.SuccessMessages.LoginRealizado 
                });
            }
            catch (Exception ex)
            {
                return HandleInternalError(ex);
            }
        }

        // Busca um usuário específico por ID
        [HttpGet("usuario/{id}")]
        public async Task<IActionResult> GetUsuario(Guid id)
        {
            try
            {
                var usuario = await _repo.GetUsuarioById(id);
                if (usuario == null)
                    return NotFound(new { message = "Usuário não encontrado" });

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return HandleInternalError(ex);
            }
        }

        // Método privado para tratar erros internos
        private IActionResult HandleInternalError(Exception ex)
        {
            return StatusCode(500, new { message = AppConstants.ErrorMessages.ErroInterno, error = ex.Message });
        }
    }
} 