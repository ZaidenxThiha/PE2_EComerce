using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using PE2.ECommerce.Domain.Data;

namespace PE2.ECommerce.Domain.Configuration;

public static class ConnectionStringFactory
{
    public const string PipeServer = @"np:\\.\pipe\LOCALDB#20734081\tsql\query";

    public static string Build(string databaseName)
        => $"Server={PipeServer};Database={databaseName};Trusted_Connection=True;MultipleActiveResultSets=True;";

    public static IConfigurationRoot CreateConfiguration(IDictionary<string, string?>? overrides = null)
    {
        var builder = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                [$"ConnectionStrings:{nameof(ECommerceDbContext)}"] = Build("PE2ECommerce"),
            });

        if (overrides is not null)
        {
            builder.AddInMemoryCollection(overrides);
        }

        return builder.Build();
    }
}
