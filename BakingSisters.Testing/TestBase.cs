using BakingSisters.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BakingSisters.Testing;

public abstract class TestBase : IDisposable
{
    protected readonly BakeryDbContext Context;
    protected readonly IConfiguration Configuration;

    protected TestBase()
    {
        var options = new DbContextOptionsBuilder<BakeryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        Context = new BakeryDbContext(options);

        Configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                {"Jwt:Key", "TestSecretKeyForUnitTestingPurposesOnly"}
            })
            .Build();
    }

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
} 