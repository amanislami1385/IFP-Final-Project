using System.Text.Json;
using Microsoft.JSInterop;

namespace AfghanFutureHope.Services;

/// <summary>Abstraction over browser storage so the logic can be unit-tested without a browser.</summary>
public interface IStorageService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value);
}

public class StorageService : IStorageService
{
    private readonly IJSRuntime _js;
    public StorageService(IJSRuntime js) => _js = js;

    public async Task<T?> GetAsync<T>(string key)
    {
        try
        {
            var json = await _js.InvokeAsync<string?>("afh.get", key);
            return string.IsNullOrWhiteSpace(json) ? default : JsonSerializer.Deserialize<T>(json);
        }
        catch (Exception ex) when (ex is JSException or JsonException or InvalidOperationException)
        {
            Console.WriteLine($"Storage read failed for '{key}': {ex.Message}");
            return default; // corrupted or blocked storage -> fall back to defaults
        }
    }

    public async Task SetAsync<T>(string key, T value)
    {
        try
        {
            await _js.InvokeVoidAsync("afh.set", key, JsonSerializer.Serialize(value));
        }
        catch (Exception ex) when (ex is JSException or InvalidOperationException)
        {
            Console.WriteLine($"Storage write failed for '{key}': {ex.Message}");
        }
    }
}
