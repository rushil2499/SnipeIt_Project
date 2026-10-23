using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace SnipeIt.Tests.Tests;

public abstract class BaseTest : PageTest
{
    private ITracing? _tracing;

    [SetUp]
    public async Task SetupTestAsync()
    {
        // Start Playwright Tracing per test for deep debugging
        _tracing = Context.Tracing;
        await _tracing.StartAsync(new()
        {
            Screenshots = true,
            Snapshots = true,
            Sources = true
        });
    }

    [TearDown]
    public async Task TeardownTestAsync()
    {
        var outcome = TestContext.CurrentContext.Result.Outcome.Status;
        var testName = TestContext.CurrentContext.Test.Name;
        var artifactDirectory = Path.Combine(TestContext.CurrentContext.WorkDirectory, "TestResults");

        Directory.CreateDirectory(artifactDirectory);

        if (outcome == TestStatus.Failed)
        {
            // Capture full screenshot on failure
            var screenshotPath = Path.Combine(artifactDirectory, $"{testName}_failed.png");
            await Page.ScreenshotAsync(new() { Path = screenshotPath, FullPage = true });
            TestContext.AddTestAttachment(screenshotPath, "Failure Screenshot");

            // Save Playwright Trace for replay
            if (_tracing != null)
            {
                var tracePath = Path.Combine(artifactDirectory, $"{testName}_trace.zip");
                await _tracing.StopAsync(new() { Path = tracePath });
                TestContext.AddTestAttachment(tracePath, "Playwright Trace");
            }
        }
        else
        {
            if (_tracing != null)
            {
                await _tracing.StopAsync();
            }
        }
    }
}