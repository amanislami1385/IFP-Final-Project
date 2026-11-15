using System.Text.Json;
using AfghanFutureHope.Services;

namespace AfghanFutureHope.Tests;

/// <summary>In-memory storage used instead of the browser's localStorage.</summary>
public class FakeStorage : IStorageService
{
    public Dictionary<string, string> Data { get; } = new();

    public Task<T?> GetAsync<T>(string key) =>
        Task.FromResult(Data.TryGetValue(key, out var json) ? JsonSerializer.Deserialize<T>(json) : default);

    public Task SetAsync<T>(string key, T value)
    {
        Data[key] = JsonSerializer.Serialize(value);
        return Task.CompletedTask;
    }
}
