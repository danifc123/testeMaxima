using Npgsql;
using Dapper;
using EcommerceApi.DTOs;
using EcommerceApi.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceApi.Repositories
{
    public class ProdutoRepository
    {
        private readonly string _connectionString;

        public ProdutoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<ProdutoResponseDto>> GetProdutos()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var sql = GetProdutosQuery();
            return await connection.QueryAsync<ProdutoResponseDto>(sql);
        }

        private string GetProdutosQuery()
        {
            return @"
                SELECT 
                    p.id,
                    p.codigo,
                    p.descricao,
                    p.departamento_codigo as DepartamentoCodigo,
                    CASE 
                        WHEN p.departamento_codigo = '010' THEN 'BEBIDAS'
                        WHEN p.departamento_codigo = '020' THEN 'CONGELADOS'
                        WHEN p.departamento_codigo = '030' THEN 'LATICINIOS'
                        WHEN p.departamento_codigo = '040' THEN 'VEGETAIS'
                        ELSE 'NÃO DEFINIDO'
                    END as DepartamentoDescricao,
                    p.preco,
                    p.status,
                    p.data_criacao as DataCriacao
                FROM produto p 
                WHERE p.excluido = false
                ORDER BY p.data_criacao DESC";
        }

        public async Task<ProdutoResponseDto?> GetProdutoById(Guid id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var sql = GetProdutoByIdQuery();
            return await connection.QueryFirstOrDefaultAsync<ProdutoResponseDto>(sql, new { id });
        }

        private string GetProdutoByIdQuery()
        {
            return @"
                SELECT 
                    p.id,
                    p.codigo,
                    p.descricao,
                    p.departamento_codigo as DepartamentoCodigo,
                    CASE 
                        WHEN p.departamento_codigo = '010' THEN 'BEBIDAS'
                        WHEN p.departamento_codigo = '020' THEN 'CONGELADOS'
                        WHEN p.departamento_codigo = '030' THEN 'LATICINIOS'
                        WHEN p.departamento_codigo = '040' THEN 'VEGETAIS'
                        ELSE 'NÃO DEFINIDO'
                    END as DepartamentoDescricao,
                    p.preco,
                    p.status,
                    p.data_criacao as DataCriacao
                FROM produto p 
                WHERE p.id = @id AND p.excluido = false";
        }

        public async Task<bool> CodigoExiste(string codigo, Guid? idExcluir = null)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var sql = "SELECT COUNT(1) FROM produto WHERE codigo = @codigo AND excluido = false";
            if (idExcluir.HasValue)
            {
                sql += " AND id != @idExcluir";
            }
            
            var count = await connection.ExecuteScalarAsync<int>(sql, new { codigo, idExcluir });
            return count > 0;
        }

        public async Task<int> InsertProduto(Guid id, string codigo, string descricao, string departamento, decimal preco, bool status)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var sql = @"
                INSERT INTO produto (id, codigo, descricao, departamento_codigo, preco, status, data_criacao) 
                VALUES (@id, @codigo, @descricao, @departamento, @preco, @status, @dataCriacao)";
            
            return await connection.ExecuteAsync(sql, new { 
                id, 
                codigo, 
                descricao, 
                departamento, 
                preco, 
                status, 
                dataCriacao = DateTime.UtcNow 
            });
        }

        public async Task<int> UpdateProduto(Guid id, string descricao, string departamento, decimal preco, bool status)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var sql = @"
                UPDATE produto 
                SET descricao = @descricao, 
                    departamento_codigo = @departamento,
                    preco = @preco, 
                    status = @status,
                    data_atualizacao = @dataAtualizacao 
                WHERE id = @id";
            
            return await connection.ExecuteAsync(sql, new { 
                id, 
                descricao, 
                departamento,
                preco, 
                status, 
                dataAtualizacao = DateTime.UtcNow 
            });
        }

        public async Task<int> DeleteProduto(Guid id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var sql = "UPDATE produto SET excluido = true, data_exclusao = @dataExclusao WHERE id = @id";
            return await connection.ExecuteAsync(sql, new { id, dataExclusao = DateTime.UtcNow });
        }
    }
}
