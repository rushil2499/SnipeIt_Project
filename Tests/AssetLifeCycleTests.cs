using FluentAssertions;
using Microsoft.Playwright;
using NUnit.Framework;
using SnipeIt.Tests.Pages;

namespace SnipeIt.Tests.Tests;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class AssetLifecycleTests : BaseTest
{
    private LoginPage _loginPage = null!;
    private CreateAssetPage _createAssetPage = null!;
    private AssetListPage _assetListPage = null!;
    private AssetDetailsPage _assetDetailsPage = null!;

    [SetUp]
    public void InitializePages()
    {
        _loginPage = new LoginPage(Page);
        _createAssetPage = new CreateAssetPage(Page);
        _assetListPage = new AssetListPage(Page);
        _assetDetailsPage = new AssetDetailsPage(Page);
    }

    [Test]
    [Description("Logs in, creates a Macbook Pro 13\" asset, verifies presence in asset list, and validates details and checkout audit entry under History.")]
    public async Task Should_CreateMacbookProAsset_AssignToUser_AndVerifyInHistory()
    {
        // -------------------------------------------------------------
        // Step 1: Login to Snipe-IT demo
        // -------------------------------------------------------------
        await _loginPage.NavigateAsync();
        await _loginPage.LoginAsync("admin", "password");

        // -------------------------------------------------------------
        // Step 2: Create a new Macbook Pro 13" asset & checkout to a random user
        // -------------------------------------------------------------
        await _createAssetPage.NavigateAsync();
        var createdAsset = await _createAssetPage.CreateAssetAndCheckoutAsync(
            modelName: "Macbook Pro 13\"",
            statusName: "Ready to Deploy"
        );

        createdAsset.AssetTag.Should().NotBeNullOrWhiteSpace("an asset tag should be assigned");
        createdAsset.AssignedUser.Should().NotBeNullOrWhiteSpace("a user should be picked for checkout");

        // -------------------------------------------------------------
        // Step 3: Find the created asset in the assets list
        // -------------------------------------------------------------
        await _assetListPage.NavigateAsync();
        await _assetListPage.SearchAndOpenAssetAsync(createdAsset.AssetTag);

        // Verify URL transitioned to the hardware view page
        Page.Url.Should().Contain("/hardware/", "navigating from list should land on asset details");

        // -------------------------------------------------------------
        // Step 4: Validate asset details on the Asset Page
        // -------------------------------------------------------------
        var pageContent = await _assetDetailsPage.GetPageTextAsync();

        pageContent.Should().Contain(createdAsset.AssetTag, "the asset tag must be present on the page");
        pageContent.Should().Contain("Macbook Pro 13\"", "the asset model must be present");
        pageContent.Should().Contain("Ready to Deploy", "the asset status must be displayed");

        // Normalize user display text (Snipe-IT displays first/last name)
        var userFirstWord = createdAsset.AssignedUser.Split(new[] { ' ', '(' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? "";
        pageContent.Should().Contain(userFirstWord, "the assigned user name should be displayed in the asset summary");

        // -------------------------------------------------------------
        // Step 5: Validate checkout audit log in the History tab
        // -------------------------------------------------------------
        await _assetDetailsPage.OpenHistoryTabAsync();

        var historyTableText = await Page.Locator("#history").InnerTextAsync();

        // Validates the asset creation was audited
        historyTableText.Should().ContainEquivalentOf("create new", "the audit log must record asset creation");
        historyTableText.Should().Contain(createdAsset.AssetTag, "the audit log must reference the unique asset tag");
    }
}