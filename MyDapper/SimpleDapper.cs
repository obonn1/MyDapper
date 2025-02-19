using System.Data.Common;

namespace MyDapper;

public class SimpleDapper(DbConnection connection)
{
    public async Task<int> ExecuteAsync(string sql)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = sql;
        return await command.ExecuteNonQueryAsync();
    }
}
