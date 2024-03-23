using System.Data;
using Npgsql;

namespace TogetherBoardsApp.Backend.Infrastructure.Database.SqlConnection;

internal sealed class SqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        return connection;
    }
}