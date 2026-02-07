using ExampleProject.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace ExampleProject.Api.Tests;

/// <summary>
/// Factory for integration tests: boots the API (Blazor + endpoints) in-memory.
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        return base.CreateHost(builder);
    }
}
