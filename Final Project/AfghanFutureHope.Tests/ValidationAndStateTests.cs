using System.ComponentModel.DataAnnotations;
using AfghanFutureHope.Models;
using AfghanFutureHope.Services;
using Bunit;
using Xunit;

namespace AfghanFutureHope.Tests;

public class ValidationAndStateTests
{
    private static List<ValidationResult> Validate(object model)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(model, new ValidationContext(model), results, validateAllProperties: true);
        return results;
    }

    [Fact]
    public void DonationForm_RejectsBadInput()
    {
        var form = new DonationForm { Donor = "", Email = "not-an-email", Amount = 0 };
        Assert.True(Validate(form).Count >= 3);
    }

    [Fact]
    public void DonationForm_AcceptsGoodInput()
    {
        var form = new DonationForm { Donor = "Ali Ahmadi", Email = "ali@example.com", Amount = 20 };
        Assert.Empty(Validate(form));
    }

    [Fact]
    public void ApplicationForm_RequiresLongStatement()
    {
        var form = new ApplicationForm { Name = "Sara", Email = "sara@example.com", Statement = "short" };
        Assert.NotEmpty(Validate(form));
    }

    [Theory]
    [InlineData("en", "nav.home", "Home")]
    [InlineData("fa", "nav.home", "خانه")]
    [InlineData("ps", "nav.home", "کور")]
    [InlineData("xx", "nav.home", "Home")]       // unknown language falls back to English
    [InlineData("en", "missing.key", "missing.key")] // unknown key returns the key
    public void Localizer_ReturnsExpectedText(string lang, string key, string expected) =>
        Assert.Equal(expected, Localizer.Get(lang, key));

    [Fact]
    public async Task Auth_SignsInAdmin_AndRejectsWrongPassword()
    {
        using var ctx = new TestContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        var state = new AppState(new FakeStorage(), ctx.JSInterop.JSRuntime);
        var auth = new AuthService(state);

        Assert.False(await auth.SignInAsync("admin", "wrong"));
        Assert.False(state.IsSignedIn);

        Assert.True(await auth.SignInAsync("admin", "admin123"));
        Assert.True(state.IsAdmin);

        await auth.SignOutAsync();
        Assert.False(state.IsSignedIn);
    }

    [Fact]
    public async Task AppState_SwitchesToRtlForDari()
    {
        using var ctx = new TestContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        var state = new AppState(new FakeStorage(), ctx.JSInterop.JSRuntime);

        await state.SetLanguageAsync("fa");
        Assert.Equal("rtl", state.Dir);
        await state.SetLanguageAsync("en");
        Assert.Equal("ltr", state.Dir);
    }
}
