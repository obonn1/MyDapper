using System.Data.Common;
using Microsoft.Data.SqlClient;
using Shouldly;

namespace MyDapper.Tests;

[TestFixture]
public class SimpleDapperTests
{
    private DbConnection? connection;
    private SimpleDapper? simpleDapper;

    public static readonly string ConnectionString = new SqlConnectionStringBuilder
    {
        DataSource = "(local)",
        InitialCatalog = "MyDapperTest",
        IntegratedSecurity = true,
        TrustServerCertificate = true
    }.ToString();

    [SetUp]
    public async Task Setup()
    {
        connection = new SqlConnection(ConnectionString);
        simpleDapper = new SimpleDapper(connection);

        const string createTestTableSql =
            """
            if object_id('TestTable') is not null drop table TestTable;
            create table TestTable (Id int primary key, Name nvarchar(50));
            """;

        await simpleDapper.ExecuteAsync(createTestTableSql);
    }

    [TearDown]
    public async Task TearDown()
    {
        const string dropTestTableSql = "if object_id('TestTable') is not null drop table TestTable;";

        await simpleDapper!.ExecuteAsync(dropTestTableSql);

        await connection!.CloseAsync();
        connection.Dispose();
    }

    [Test]
    public async Task ExecuteAsync_should_affect_rows()
    {
        // Arrange
        const string insertSql = "insert into TestTable (Id, Name) values (1, 'Test')";

        // Act
        var affectedRows = await simpleDapper!.ExecuteAsync(insertSql);

        // Assert
        affectedRows.ShouldBe(1);
    }
}
