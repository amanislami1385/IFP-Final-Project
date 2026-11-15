using AfghanFutureHope.Models;

namespace AfghanFutureHope.Services;

public interface IDataService
{
    event Action? Changed;
    Task InitAsync();

    IReadOnlyList<Initiative> Initiatives { get; }
    IReadOnlyList<Opportunity> Opportunities { get; }
    IReadOnlyList<Story> Stories { get; }
    IReadOnlyList<Donation> Donations { get; }
    IReadOnlyList<JobApplication> Applications { get; }

    int TotalRaised { get; }
    int TotalBeneficiaries { get; }

    Initiative? GetInitiative(int id);
    Task<Donation> DonateAsync(int initiativeId, DonationForm form, string submittedBy = "guest");
    Task<JobApplication> ApplyAsync(int opportunityId, ApplicationForm form, string submittedBy = "guest");
    Task<Initiative> AddInitiativeAsync(InitiativeForm form);
    Task DeleteInitiativeAsync(int id);
}
