// Single app: Blazor UI + API on one host
using ExampleProject.Api.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapGet("/favicon.ico", () => Results.NoContent());
app.MapGet("/api", () => "ExampleProject.Api");

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
