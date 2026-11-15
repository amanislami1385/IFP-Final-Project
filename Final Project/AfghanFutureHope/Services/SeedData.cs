using AfghanFutureHope.Models;

namespace AfghanFutureHope.Services;

/// <summary>Sample data for the demo. Replace with an API or database in production.</summary>
public static class SeedData
{
    public static List<Initiative> Initiatives() => new()
    {
        new() { Id = 1, Title = "Hope Classrooms", Category = Category.Education, Province = "Kabul", Beneficiaries = 1200, Goal = 40000, Raised = 27800, Icon = "📚", Featured = true,
            Summary = "Community learning rooms with trained teachers, books and safe study space.",
            Description = "Hope Classrooms turns spare rooms and community halls into small learning centers. Funds pay for teacher stipends, textbooks, desks and winter heating so that learning does not stop when the weather turns." },
        new() { Id = 2, Title = "Girls Who Code", Category = Category.Technology, Province = "Herat", Beneficiaries = 340, Goal = 25000, Raised = 18600, Icon = "💻", Featured = true,
            Summary = "A six-month online programming course with laptops and mentors for young women.",
            Description = "Students learn web development and data skills with remote mentors. Each learner receives a laptop, an internet stipend and help finding freelance work after graduation." },
        new() { Id = 3, Title = "Mobile Health Clinics", Category = Category.Health, Province = "Bamyan", Beneficiaries = 5400, Goal = 60000, Raised = 31200, Icon = "🩺", Featured = true,
            Summary = "Vans that bring vaccines, maternal care and basic medicine to remote valleys.",
            Description = "Two vans visit villages on a fixed monthly schedule. Each visit includes a nurse, a midwife, vaccines, and a pharmacy of essential medicines." },
        new() { Id = 4, Title = "Saffron Growers Cooperative", Category = Category.Livelihood, Province = "Herat", Beneficiaries = 280, Goal = 30000, Raised = 22500, Icon = "🌾",
            Summary = "Training, tools and fair-price contracts for saffron farming families.",
            Description = "Farmers receive bulbs, drying equipment and training in quality grading, then sell through a cooperative that negotiates fair export prices." },
        new() { Id = 5, Title = "Women Artisan Studio", Category = Category.Women, Province = "Mazar-i-Sharif", Beneficiaries = 150, Goal = 18000, Raised = 9400, Icon = "🧵",
            Summary = "A home-based workshop for embroidery, carpet weaving and online selling.",
            Description = "Artisans receive materials, design coaching and a shared online shop so their handmade work reaches customers around the world." },
        new() { Id = 6, Title = "Solar Libraries", Category = Category.Education, Province = "Balkh", Beneficiaries = 2100, Goal = 22000, Raised = 14100, Icon = "☀️",
            Summary = "Solar-powered reading rooms with lights, tablets and a local library.",
            Description = "Each library runs on solar panels, so students can read after dark. A librarian from the village is trained and paid to run it." },
        new() { Id = 7, Title = "Clean Water Wells", Category = Category.Health, Province = "Ghazni", Beneficiaries = 3800, Goal = 35000, Raised = 12900, Icon = "💧",
            Summary = "Hand-pump wells and water testing for villages without safe drinking water.",
            Description = "Wells are dug with local crews and tested every quarter. A village committee is trained to maintain the pump." },
        new() { Id = 8, Title = "Online Tutoring Network", Category = Category.Education, Province = "Nangarhar", Beneficiaries = 900, Goal = 12000, Raised = 8300, Icon = "🎓",
            Summary = "Volunteer tutors teach math, English and science by video and chat.",
            Description = "Volunteers around the world are matched with students for weekly sessions. Data packages are covered for students who need them." },
    };

    public static List<Opportunity> Opportunities()
    {
        var d = DateTime.Today;
        return new()
        {
            new() { Id = 1, Title = "Undergraduate Scholarship in Computer Science", Organization = "AFH Partner Universities", Type = OpportunityType.Scholarship, Location = "Remote / Online", Deadline = d.AddDays(45), Description = "Full tuition support for a four-year degree taught online. Open to high-school graduates with strong math grades." },
            new() { Id = 2, Title = "Junior Web Developer", Organization = "Kabul Digital Studio", Type = OpportunityType.Job, Location = "Kabul", Deadline = d.AddDays(21), Description = "Build websites for local businesses using C#, .NET and Blazor. Mentoring is included." },
            new() { Id = 3, Title = "Community Health Intern", Organization = "Bamyan Health Network", Type = OpportunityType.Internship, Location = "Bamyan", Deadline = d.AddDays(30), Description = "Support mobile clinic teams with patient registration, health education and data entry." },
            new() { Id = 4, Title = "English for Work: 12-week Course", Organization = "Hope Language Lab", Type = OpportunityType.Training, Location = "Remote / Online", Deadline = d.AddDays(14), Description = "Evening classes that build workplace English for interviews, email and meetings." },
            new() { Id = 5, Title = "Graduate Fellowship in Public Health", Organization = "Global Health Fellows", Type = OpportunityType.Scholarship, Location = "Abroad", Deadline = d.AddDays(90), Description = "Two-year funded master's program with travel and living costs covered." },
            new() { Id = 6, Title = "Data Entry and Reporting Assistant", Organization = "Herat Agricultural Union", Type = OpportunityType.Job, Location = "Herat", Deadline = d.AddDays(10), Description = "Maintain farmer records and produce monthly reports in Excel." },
        };
    }

    public static List<Story> Stories() => new()
    {
        new() { Id = 1, Name = "Mariam", Role = "Web development graduate", Province = "Herat", Quote = "I learned to code at my kitchen table. Today I build websites for three clients and teach my younger sister." },
        new() { Id = 2, Name = "Hamid", Role = "Village health worker", Province = "Bamyan", Quote = "When the clinic van arrives, the whole village lines up. Children are vaccinated close to home." },
        new() { Id = 3, Name = "Zarghona", Role = "Saffron farmer", Province = "Herat", Quote = "The cooperative gave us fair prices for the first time. I paid school fees for all four of my children." },
    };
}
