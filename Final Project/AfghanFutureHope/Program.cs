using AfghanFutureHope;
using AfghanFutureHope.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Dependency injection: every service is registered against an abstraction (SOLID - D).
builder.Services.AddScoped<IStorageService, StorageService>();
builder.Services.AddScoped<IDataService, DataService>();
builder.Services.AddScoped<AppState>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ToastService>();

await builder.Build().RunAsync();
