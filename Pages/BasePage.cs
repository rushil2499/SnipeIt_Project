using Microsoft.Playwright;

namespace SnipeIt.Tests.Pages;

public abstract class BasePage
{
    protected readonly IPage Page;

    protected BasePage(IPage page)
    {
        Page = page;
    }

    // Helper for Snipe-IT Select2 dropdown search & select
    protected async Task SelectDropdownOptionAsync(ILocator selectContainer, string searchTerm)
    {
        await selectContainer.ClickAsync();

        // Select2 input appears either inside the container or in a global dropdown menu
        var searchInput = Page.Locator(".select2-container--open .select2-search__field, input.select2-search__field");
        await searchInput.WaitForAsync(new() { State = WaitForSelectorState.Visible });
        await searchInput.FillAsync(searchTerm);

        // Wait for results list and pick the first matching result
        var firstResult = Page.Locator(".select2-results__option").Filter(new() { HasText = searchTerm }).First;
        await firstResult.WaitForAsync(new() { State = WaitForSelectorState.Visible });
        await firstResult.ClickAsync();
    }
}