using Microsoft.Playwright;

namespace SnipeIt.Tests.Pages;

public class AssetDetailsPage : BasePage
{
    private readonly ILocator _historyTabLink;
    private readonly ILocator _historyTableRows;

    public AssetDetailsPage(IPage page) : base(page)
    {
        _historyTabLink = Page.Locator("a[href='#history'], a:has-text('History')").First;
        // Specifically match table rows that are NOT the placeholder "no records" / "loading" row
        _historyTableRows = Page.Locator("#history table tbody tr:not(.no-records-found)");
    }

    public async Task<string> GetPageTextAsync()
    {
        return await Page.Locator("body").InnerTextAsync();
    }

    public async Task OpenHistoryTabAsync()
    {
        await _historyTabLink.WaitForAsync(new() { State = WaitForSelectorState.Visible });
        await _historyTabLink.ClickAsync();

        // Wait until the "No matching records found" / "Loading" placeholder disappears or the real row appears
        var firstValidRow = _historyTableRows.First;
        await firstValidRow.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 10000 });
    }

    public async Task<ILocator> GetHistoryRowsAsync()
    {
        await _historyTableRows.First.WaitForAsync(new() { State = WaitForSelectorState.Visible });
        return _historyTableRows;
    }
}