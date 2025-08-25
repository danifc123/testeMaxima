using Npgsql;
using Dapper;
using EcommerceApi.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceApi.Repositories
{
    public class UsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<UsuarioResponseDto?> GetUsuarioByEmail(string email)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var sql = GetUsuarioByEmailQuery();
            return await connection.QueryFirstOrDefaultAsync<UsuarioResponseDto>(sql, new { email });
        }

        public async Task<UsuarioResponseDto?> GetUsuarioById(Guid id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var sql = GetUsuarioByIdQuery();
            return await connection.QueryFirstOrDefaultAsync<UsuarioResponseDto>(sql, new { id });
        }

        public async Task<bool> EmailExiste(string email)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var sql = "SELECT COUNT(1) FROM usuario WHERE email = @email AND excluido = false";
            var count = await connection.ExecuteScalarAsync<int>(sql, new { email });
            return count > 0;
        }

        public async Task<UsuarioResponseDto?> GetUsuarioByEmailAndSenha(string email, string senhaHash)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var sql = GetUsuarioByEmailAndSenhaQuery();
            return await connection.QueryFirstOrDefaultAsync<UsuarioResponseDto>(sql, new { email, senhaHash });
        }

        public async Task<int> InsertUsuario(Guid id, string nome, string email, string senhaHash)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var sql = GetInsertUsuarioQuery();
            return await connection.ExecuteAsync(sql, new { 
                id, 
                nome, 
                email, 
                senhaHash, 
                dataCriacao = DateTime.UtcNow 
            });
        }

        private string GetUsuarioByEmailQuery()
        {
            return @"
                SELECT 
                    id,
                    nome,
                    email,
                    data_criacao as DataCriacao
                FROM usuario 
                WHERE email = @email AND excluido = false";
        }

        private string GetUsuarioByIdQuery()
        {
            return @"
                SELECT 
                    id,
                    nome,
                    email,
                    data_criacao as DataCriacao
                FROM usuario 
                WHERE id = @id AND excluido = false";
        }

        private string GetUsuarioByEmailAndSenhaQuery()
        {
            return @"
                SELECT 
                    id,
                    nome,
                    email,
                    data_criacao as DataCriacao
                FROM usuario 
                WHERE email = @email AND senha_hash = @senhaHash AND excluido = false";
        }

        private string GetInsertUsuarioQuery()
        {
            return @"
                INSERT INTO usuario (id, nome, email, senha_hash, data_criacao) 
                VALUES (@id, @nome, @email, @senhaHash, @dataCriacao)";
        }
    }
} 