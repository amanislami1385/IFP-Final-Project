using AfghanFutureHope.Models;

namespace AfghanFutureHope.Services;

/// <summary>
/// In-memory repository persisted to browser storage. Swap this class for an
/// HttpClient-based implementation to connect a real REST API without touching any page.
/// </summary>
public class DataService : IDataService
{
    private const string InitiativesKey = "afh.initiatives";
    private const string DonationsKey = "afh.donations";
    private const string ApplicationsKey = "afh.applications";

    private readonly IStorageService _storage;
    private Task? _init;

    private List<Initiative> _initiatives = new();
    private List<Opportunity> _opportunities = new();
    private List<Story> _stories = new();
    private List<Donation> _donations = new();
    private List<JobApplication> _applications = new();

    public event Action? Changed;

    public DataService(IStorageService storage) => _storage = storage;

    public IReadOnlyList<Initiative> Initiatives => _initiatives;
    public IReadOnlyList<Opportunity> Opportunities => _opportunities;
    public IReadOnlyList<Story> Stories => _stories;
    public IReadOnlyList<Donation> Donations => _donations;
    public IReadOnlyList<JobApplication> Applications => _applications;

    public int TotalRaised => _initiatives.Sum(i => i.Raised);
    public int TotalBeneficiaries => _initiatives.Sum(i => i.Beneficiaries);

    // The task is cached, so concurrent callers share one load.
    public Task InitAsync() => _init ??= LoadAsync();

    private async Task LoadAsync()
    {
        _initiatives = await _storage.GetAsync<List<Initiative>>(InitiativesKey) ?? SeedData.Initiatives();
        _donations = await _storage.GetAsync<List<Donation>>(DonationsKey) ?? new();
        _applications = await _storage.GetAsync<List<JobApplication>>(ApplicationsKey) ?? new();
        _opportunities = SeedData.Opportunities();
        _stories = SeedData.Stories();
    }

    public Initiative? GetInitiative(int id) => _initiatives.FirstOrDefault(i => i.Id == id);

    public async Task<Donation> DonateAsync(int initiativeId, DonationForm form, string submittedBy = "guest")
    {
        ArgumentNullException.ThrowIfNull(form);
        if (form.Amount < 1) throw new ArgumentOutOfRangeException(nameof(form), "Amount must be at least 1.");
        var initiative = GetInitiative(initiativeId)
            ?? throw new KeyNotFoundException($"Initiative {initiativeId} was not found.");

        var donation = new Donation
        {
            Id = _donations.Count == 0 ? 1 : _donations.Max(d => d.Id) + 1,
            InitiativeId = initiativeId,
            Donor = form.Donor.Trim(),
            Amount = form.Amount,
            Message = form.Message?.Trim(),
            SubmittedBy = submittedBy,
            Date = DateTime.Now
        };

        initiative.Raised += form.Amount;
        _donations.Add(donation);
        await _storage.SetAsync(InitiativesKey, _initiatives);
        await _storage.SetAsync(DonationsKey, _donations);
        Changed?.Invoke();
        return donation;
    }

    public async Task<JobApplication> ApplyAsync(int opportunityId, ApplicationForm form, string submittedBy = "guest")
    {
        ArgumentNullException.ThrowIfNull(form);
        if (_opportunities.All(o => o.Id != opportunityId))
            throw new KeyNotFoundException($"Opportunity {opportunityId} was not found.");

        var application = new JobApplication
        {
            Id = _applications.Count == 0 ? 1 : _applications.Max(a => a.Id) + 1,
            OpportunityId = opportunityId,
            Name = form.Name.Trim(),
            Email = form.Email.Trim(),
            Statement = form.Statement.Trim(),
            SubmittedBy = submittedBy,
            Date = DateTime.Now
        };

        _applications.Add(application);
        await _storage.SetAsync(ApplicationsKey, _applications);
        Changed?.Invoke();
        return application;
    }

    public async Task<Initiative> AddInitiativeAsync(InitiativeForm form)
    {
        ArgumentNullException.ThrowIfNull(form);
        var initiative = new Initiative
        {
            Id = _initiatives.Count == 0 ? 1 : _initiatives.Max(i => i.Id) + 1,
            Title = form.Title.Trim(),
            Summary = form.Summary.Trim(),
            Description = form.Summary.Trim(),
            Province = form.Province.Trim(),
            Category = form.Category,
            Goal = form.Goal,
            Beneficiaries = form.Beneficiaries,
            Icon = form.Category switch
            {
                Category.Education => "📚", Category.Health => "🩺", Category.Livelihood => "🌾",
                Category.Technology => "💻", Category.Women => "🧵", _ => "🌱"
            }
        };
        _initiatives.Add(initiative);
        await _storage.SetAsync(InitiativesKey, _initiatives);
        Changed?.Invoke();
        return initiative;
    }

    public async Task DeleteInitiativeAsync(int id)
    {
        var item = GetInitiative(id) ?? throw new KeyNotFoundException($"Initiative {id} was not found.");
        _initiatives.Remove(item);
        await _storage.SetAsync(InitiativesKey, _initiatives);
        Changed?.Invoke();
    }
}
