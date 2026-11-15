using System.Security.Cryptography;
using System.Text;
using AfghanFutureHope.Models;

namespace AfghanFutureHope.Services;

/// <summary>
/// Demo role-based sign-in. Passwords are compared as SHA-256 hashes.
/// In production, replace with ASP.NET Core Identity / OAuth and a server API.
/// </summary>
public class AuthService
{
    private readonly AppState _state;
    public AuthService(AppState state) => _state = state;

    private static string Hash(string value) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value)));

    private static readonly Dictionary<string, (string Hash, Role Role, string Name)> Accounts =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["admin"] = (Hash("admin123"), Role.Admin, "Platform Admin"),
            ["student"] = (Hash("student123"), Role.Student, "Demo Student")
        };

    public async Task<bool> SignInAsync(string username, string password)
    {
        if (!Accounts.TryGetValue(username.Trim(), out var account)) return false;
        if (!CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(account.Hash), Encoding.UTF8.GetBytes(Hash(password)))) return false;

        await _state.SetUserAsync(new UserSession
        {
            Username = username.Trim().ToLowerInvariant(),
            DisplayName = account.Name,
            Role = account.Role
        });
        return true;
    }

    public Task SignOutAsync() => _state.SetUserAsync(null);
}
