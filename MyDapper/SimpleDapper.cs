using System.Data;
using System.Data.Common;

namespace MyDapper;

public class SimpleDapper(DbConnection connection)
{
    public async Task<int> ExecuteAsync(string sql)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = sql;

        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync();

        return await command.ExecuteNonQueryAsync();
    }
}
