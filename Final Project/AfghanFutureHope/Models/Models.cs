using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AfghanFutureHope.Models;

public enum Category { Education, Health, Livelihood, Technology, Women }
public enum OpportunityType { Scholarship, Job, Internship, Training }
public enum Role { Student, Admin }

public class Initiative
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Summary { get; set; } = "";
    public string Description { get; set; } = "";
    public Category Category { get; set; }
    public string Province { get; set; } = "";
    public int Beneficiaries { get; set; }
    public int Goal { get; set; }
    public int Raised { get; set; }
    public string Icon { get; set; } = "🌱";
    public bool Featured { get; set; }

    [JsonIgnore]
    public int Percent => Goal <= 0 ? 0 : (int)Math.Min(100, Math.Round(Raised * 100.0 / Goal));
}

public class Opportunity
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Organization { get; set; } = "";
    public OpportunityType Type { get; set; }
    public string Location { get; set; } = "";
    public DateTime Deadline { get; set; }
    public string Description { get; set; } = "";
}

public class Story
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Role { get; set; } = "";
    public string Province { get; set; } = "";
    public string Quote { get; set; } = "";
}

public class Donation
{
    public int Id { get; set; }
    public int InitiativeId { get; set; }
    public string Donor { get; set; } = "";
    public string SubmittedBy { get; set; } = "guest";
    public int Amount { get; set; }
    public string? Message { get; set; }
    public DateTime Date { get; set; }
}

public class JobApplication
{
    public int Id { get; set; }
    public int OpportunityId { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Statement { get; set; } = "";
    public string SubmittedBy { get; set; } = "guest";
    public DateTime Date { get; set; }
}

public class UserSession
{
    public string Username { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public Role Role { get; set; }
}

// ---------- Form models (validated with DataAnnotations) ----------

public class DonationForm
{
    [Required(ErrorMessage = "Enter your name.")]
    [StringLength(60, MinimumLength = 2, ErrorMessage = "Use between 2 and 60 characters.")]
    public string Donor { get; set; } = "";

    [Required(ErrorMessage = "Enter your email.")]
    [EmailAddress(ErrorMessage = "Enter a valid email, like name@example.com.")]
    public string Email { get; set; } = "";

    [Range(1, 100000, ErrorMessage = "Enter an amount between $1 and $100,000.")]
    public int Amount { get; set; } = 25;

    [StringLength(200, ErrorMessage = "Keep the message under 200 characters.")]
    public string? Message { get; set; }
}

public class ApplicationForm
{
    [Required(ErrorMessage = "Enter your full name.")]
    [StringLength(80, MinimumLength = 2)]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Enter your email.")]
    [EmailAddress(ErrorMessage = "Enter a valid email, like name@example.com.")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Tell us why you are a good fit.")]
    [MinLength(20, ErrorMessage = "Write at least 20 characters.")]
    [StringLength(600, ErrorMessage = "Keep it under 600 characters.")]
    public string Statement { get; set; } = "";
}

public class InitiativeForm
{
    [Required(ErrorMessage = "Enter a title.")]
    [StringLength(60, MinimumLength = 3)]
    public string Title { get; set; } = "";

    [Required(ErrorMessage = "Enter a short summary.")]
    [StringLength(160, MinimumLength = 10)]
    public string Summary { get; set; } = "";

    [Required(ErrorMessage = "Enter a province.")]
    public string Province { get; set; } = "";

    public Category Category { get; set; } = Category.Education;

    [Range(100, 1000000, ErrorMessage = "Goal must be between $100 and $1,000,000.")]
    public int Goal { get; set; } = 5000;

    [Range(1, 1000000, ErrorMessage = "Enter the number of people helped.")]
    public int Beneficiaries { get; set; } = 100;
}

public class LoginForm
{
    [Required(ErrorMessage = "Enter your username.")]
    public string Username { get; set; } = "";

    [Required(ErrorMessage = "Enter your password.")]
    public string Password { get; set; } = "";
}

public class ContactForm
{
    [Required(ErrorMessage = "Enter your name.")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Enter your email.")]
    [EmailAddress(ErrorMessage = "Enter a valid email, like name@example.com.")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Write a message.")]
    [MinLength(10, ErrorMessage = "Write at least 10 characters.")]
    public string Message { get; set; } = "";
}
