namespace AfghanFutureHope.Services;

/// <summary>Small dictionary-based localizer: English, Dari (fa) and Pashto (ps).</summary>
public static class Localizer
{
    public static readonly IReadOnlyDictionary<string, string> Languages = new Dictionary<string, string>
    {
        ["en"] = "English", ["fa"] = "دری", ["ps"] = "پښتو"
    };

    private static readonly Dictionary<string, Dictionary<string, string>> Text = new()
    {
        ["en"] = new()
        {
            ["nav.home"] = "Home", ["nav.initiatives"] = "Initiatives", ["nav.opportunities"] = "Opportunities",
            ["nav.stories"] = "Stories", ["nav.about"] = "About", ["nav.login"] = "Sign in",
            ["nav.dashboard"] = "Dashboard", ["nav.logout"] = "Sign out",
            ["hero.title"] = "Every child deserves a tomorrow worth studying for.",
            ["hero.sub"] = "Afghan Future Hope connects donors, volunteers and young people with schools, clinics, scholarships and jobs that change lives.",
            ["hero.cta1"] = "Support an initiative", ["hero.cta2"] = "Find an opportunity"
        },
        ["fa"] = new()
        {
            ["nav.home"] = "خانه", ["nav.initiatives"] = "ابتکارها", ["nav.opportunities"] = "فرصت‌ها",
            ["nav.stories"] = "داستان‌ها", ["nav.about"] = "درباره ما", ["nav.login"] = "ورود",
            ["nav.dashboard"] = "داشبورد", ["nav.logout"] = "خروج",
            ["hero.title"] = "هر کودک سزاوار فردایی است که ارزش درس خواندن را داشته باشد.",
            ["hero.sub"] = "امید آینده افغانستان، حامیان، داوطلبان و جوانان را با مکتب‌ها، کلینیک‌ها، بورسیه‌ها و کارهایی که زندگی را تغییر می‌دهند وصل می‌کند.",
            ["hero.cta1"] = "از یک ابتکار حمایت کنید", ["hero.cta2"] = "فرصت پیدا کنید"
        },
        ["ps"] = new()
        {
            ["nav.home"] = "کور", ["nav.initiatives"] = "نوښتونه", ["nav.opportunities"] = "فرصتونه",
            ["nav.stories"] = "کیسې", ["nav.about"] = "زموږ په اړه", ["nav.login"] = "ننوتل",
            ["nav.dashboard"] = "ډشبورډ", ["nav.logout"] = "وتل",
            ["hero.title"] = "هر ماشوم د داسې سبا حق لري چې د زده کړې ارزښت ولري.",
            ["hero.sub"] = "د افغانستان د راتلونکي هیله ملاتړي، رضاکاران او ځوانان له ښوونځیو، کلینیکونو، بورسونو او هغو کارونو سره نښلوي چې ژوند بدلوي.",
            ["hero.cta1"] = "له یوه نوښت ملاتړ وکړئ", ["hero.cta2"] = "فرصت ومومئ"
        }
    };

    public static string Get(string lang, string key)
    {
        if (Text.TryGetValue(lang, out var table) && table.TryGetValue(key, out var value)) return value;
        return Text["en"].TryGetValue(key, out var fallback) ? fallback : key;
    }
}
