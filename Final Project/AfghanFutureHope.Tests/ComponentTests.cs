using AfghanFutureHope.Components;
using AfghanFutureHope.Models;
using Bunit;
using Xunit;

namespace AfghanFutureHope.Tests;

public class ComponentTests
{
    [Fact]
    public void StatCard_RendersValueAndLabel()
    {
        using var ctx = new TestContext();
        var cut = ctx.RenderComponent<StatCard>(p => p
            .Add(x => x.Value, "1,200")
            .Add(x => x.Label, "people reached"));

        Assert.Contains("1,200", cut.Markup);
        Assert.Contains("people reached", cut.Markup);
    }

    [Fact]
    public void ProgressBar_ExposesAriaValue()
    {
        using var ctx = new TestContext();
        var cut = ctx.RenderComponent<ProgressBar>(p => p.Add(x => x.Percent, 64));
        Assert.Equal("64", cut.Find("[role=progressbar]").GetAttribute("aria-valuenow"));
    }

    [Fact]
    public void TemplatedList_ShowsEmptyTemplate_WhenNoItems()
    {
        using var ctx = new TestContext();
        var cut = ctx.RenderComponent<TemplatedList<string>>(p => p
            .Add(x => x.Items, Array.Empty<string>())
            .Add(x => x.ItemTemplate, s => $"<span>{s}</span>")
            .Add(x => x.Empty, "<p id='empty'>Nothing here</p>"));

        Assert.NotNull(cut.Find("#empty"));
    }

    [Fact]
    public void TemplatedList_RendersEachItem()
    {
        using var ctx = new TestContext();
        var cut = ctx.RenderComponent<TemplatedList<string>>(p => p
            .Add(x => x.Items, new[] { "a", "b", "c" })
            .Add(x => x.ItemTemplate, s => $"<span class='i'>{s}</span>"));

        Assert.Equal(3, cut.FindAll(".i").Count);
    }

    [Fact]
    public void InitiativeCard_ShowsTitleAndRaisesSupportCallback()
    {
        using var ctx = new TestContext();
        Initiative? clicked = null;
        var item = new Initiative { Id = 7, Title = "Solar Library", Category = Category.Education, Goal = 100, Raised = 40 };

        var cut = ctx.RenderComponent<InitiativeCard>(p => p
            .Add(x => x.Item, item)
            .Add(x => x.OnSupport, (Initiative i) => clicked = i));

        Assert.Contains("Solar Library", cut.Markup);
        cut.Find("button").Click();
        Assert.Same(item, clicked);
    }
}
