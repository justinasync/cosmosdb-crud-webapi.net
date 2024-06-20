using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace CosmosCrud.Api.Tests;

public static class Reusables
{
    public static readonly Action<IWebHostBuilder> HostConfiguration = builder =>
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            const string env = "Testing";
            context.HostingEnvironment.EnvironmentName = env;

            config.Sources.Clear();
            config
                .AddJsonFile($"appsettings.{env}.json", optional: false, reloadOnChange: false)
                .AddEnvironmentVariables();
        });
    };
}
