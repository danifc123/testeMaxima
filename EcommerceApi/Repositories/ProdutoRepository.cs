using Npgsql;
using Dapper; // ajuda nas queries (se permitido)
using System.Collections.Generic;
using System.Threading.Tasks;

public class ProdutoRepository
{
    private readonly string _connectionString;

    public ProdutoRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<dynamic>> GetProdutos()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        var sql = "SELECT * FROM produto WHERE excluido = false";
        return await connection.QueryAsync(sql);
    }

    public async Task<int> InsertProduto(Guid id, string codigo, string descricao, string departamento, decimal preco, bool status)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        var sql = @"INSERT INTO produto (id, codigo, descricao, departamento_codigo, preco, status) 
                    VALUES (@id, @codigo, @descricao, @departamento, @preco, @status)";
        return await connection.ExecuteAsync(sql, new { id, codigo, descricao, departamento, preco, status });
    }

    public async Task<int> UpdateProduto(Guid id, string descricao, decimal preco, bool status)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        var sql = @"UPDATE produto SET descricao=@descricao, preco=@preco, status=@status WHERE id=@id";
        return await connection.ExecuteAsync(sql, new { id, descricao, preco, status });
    }

    public async Task<int> DeleteProduto(Guid id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        var sql = @"UPDATE produto SET excluido=true WHERE id=@id";
        return await connection.ExecuteAsync(sql, new { id });
    }
}
