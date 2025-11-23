using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;

namespace BinView.Tests;

public class FileReadTest : PageTest
{
    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions()
        {
            ColorScheme = ColorScheme.Light,
            ViewportSize = new()
            {
                Width = 1920,
                Height = 1080
            },
            BaseURL = "http://localhost:5163",
        };
    }

    [Fact]
    public async Task ReadFile()
    {
        await Page.GotoAsync("/");

        var fileInput = Page.Locator("#file-selector");
        await fileInput.SetInputFilesAsync("Fixtures/empty.zip");

        // Click submit button
        var submitButton = Page.Locator("button");
        await submitButton.ClickAsync();

        // Wait for the result to appear
        var firstByte = Page.Locator(".hex-view-line").First.Locator("span").First;
        var firstByteStr = await firstByte.InnerTextAsync();

        Assert.Equal("50", firstByteStr);
    }
}
