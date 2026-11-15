using AfghanFutureHope.Models;
using AfghanFutureHope.Services;
using Xunit;

namespace AfghanFutureHope.Tests;

public class DataServiceTests
{
    private static async Task<(DataService Service, FakeStorage Storage)> CreateAsync()
    {
        var storage = new FakeStorage();
        var service = new DataService(storage);
        await service.InitAsync();
        return (service, storage);
    }

    private static DonationForm ValidDonation(int amount = 50) =>
        new() { Donor = "Ali Ahmadi", Email = "ali@example.com", Amount = amount };

    [Fact]
    public async Task Init_LoadsSeedData()
    {
        var (service, _) = await CreateAsync();
        Assert.NotEmpty(service.Initiatives);
        Assert.NotEmpty(service.Opportunities);
        Assert.NotEmpty(service.Stories);
        Assert.True(service.TotalRaised > 0);
    }

    [Fact]
    public async Task Donate_IncreasesRaised_Persists_AndNotifies()
    {
        var (service, storage) = await CreateAsync();
        var before = service.GetInitiative(1)!.Raised;
        var notified = false;
        service.Changed += () => notified = true;

        var donation = await service.DonateAsync(1, ValidDonation(75));

        Assert.Equal(before + 75, service.GetInitiative(1)!.Raised);
        Assert.Equal(75, donation.Amount);
        Assert.True(notified);
        Assert.True(storage.Data.ContainsKey("afh.donations"));
    }

    [Fact]
    public async Task Donate_UnknownInitiative_Throws()
    {
        var (service, _) = await CreateAsync();
        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.DonateAsync(999, ValidDonation()));
    }

    [Fact]
    public async Task Donate_ZeroAmount_Throws()
    {
        var (service, _) = await CreateAsync();
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.DonateAsync(1, ValidDonation(0)));
    }

    [Fact]
    public async Task Data_SurvivesReload()
    {
        var (service, storage) = await CreateAsync();
        await service.DonateAsync(2, ValidDonation(100));

        var reloaded = new DataService(storage);
        await reloaded.InitAsync();

        Assert.Single(reloaded.Donations);
        Assert.Equal(service.GetInitiative(2)!.Raised, reloaded.GetInitiative(2)!.Raised);
    }

    [Fact]
    public async Task AddAndDeleteInitiative_Work()
    {
        var (service, _) = await CreateAsync();
        var count = service.Initiatives.Count;

        var created = await service.AddInitiativeAsync(new InitiativeForm
        {
            Title = "Test Library", Summary = "A test library for students", Province = "Kabul", Goal = 1000, Beneficiaries = 50
        });
        Assert.Equal(count + 1, service.Initiatives.Count);

        await service.DeleteInitiativeAsync(created.Id);
        Assert.Equal(count, service.Initiatives.Count);
        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.DeleteInitiativeAsync(created.Id));
    }

    [Fact]
    public async Task Apply_StoresApplication()
    {
        var (service, _) = await CreateAsync();
        var app = await service.ApplyAsync(1, new ApplicationForm
        {
            Name = "Sara", Email = "sara@example.com", Statement = "I love mathematics and teaching."
        }, "student");

        Assert.Equal("student", app.SubmittedBy);
        Assert.Single(service.Applications);
    }

    [Fact]
    public void Percent_IsCappedAt100()
    {
        var item = new Initiative { Goal = 100, Raised = 250 };
        Assert.Equal(100, item.Percent);
        Assert.Equal(0, new Initiative { Goal = 0, Raised = 10 }.Percent);
    }
}
