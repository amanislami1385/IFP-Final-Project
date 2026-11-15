using AfghanFutureHope.Models;
using Microsoft.JSInterop;

namespace AfghanFutureHope.Services;

/// <summary>
/// State container: one place for theme, language and the signed-in user.
/// Components subscribe to <see cref="Changed"/> or receive it as a CascadingValue.
/// </summary>
public class AppState
{
    private readonly IStorageService _storage;
    private readonly IJSRuntime _js;
    private bool _initialized;

    public AppState(IStorageService storage, IJSRuntime js)
    {
        _storage = storage;
        _js = js;
    }

    public event Action? Changed;

    public string Theme { get; private set; } = "light";
    public string Lang { get; private set; } = "en";
    public UserSession? User { get; private set; }

    public bool IsSignedIn => User is not null;
    public bool IsAdmin => User?.Role == Role.Admin;
    public string Dir => Lang == "en" ? "ltr" : "rtl";

    public string T(string key) => Localizer.Get(Lang, key);

    public async Task InitAsync()
    {
        if (_initialized) return;
        _initialized = true;

        var prefersDark = false;
        try { prefersDark = await _js.InvokeAsync<bool>("afh.prefersDark"); } catch (JSException) { }

        Theme = await _storage.GetAsync<string>("afh.theme") ?? (prefersDark ? "dark" : "light");
        var lang = await _storage.GetAsync<string>("afh.lang");
        Lang = lang is not null && Localizer.Languages.ContainsKey(lang) ? lang : "en";
        User = await _storage.GetAsync<UserSession>("afh.user");
        await ApplyAsync();
    }

    public async Task ToggleThemeAsync()
    {
        Theme = Theme == "dark" ? "light" : "dark";
        await _storage.SetAsync("afh.theme", Theme);
        await ApplyAsync();
        Changed?.Invoke();
    }

    public async Task SetLanguageAsync(string lang)
    {
        if (!Localizer.Languages.ContainsKey(lang)) return;
        Lang = lang;
        await _storage.SetAsync("afh.lang", Lang);
        await ApplyAsync();
        Changed?.Invoke();
    }

    public async Task SetUserAsync(UserSession? user)
    {
        User = user;
        await _storage.SetAsync("afh.user", user);
        Changed?.Invoke();
    }

    private async Task ApplyAsync()
    {
        try { await _js.InvokeVoidAsync("afh.applyPrefs", Theme, Lang, Dir); } catch (JSException) { }
    }
}
