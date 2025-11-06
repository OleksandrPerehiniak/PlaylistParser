using Microsoft.Playwright;
using PlaylistParser.Services.Abstractions;
using System.Threading.Tasks;

namespace PlaylistParser.Services.HtmlLoaders
{
    internal class HtmlLoader : IHtmlLoader
    {
        public async Task<string> FetchRenderedHtmlAsync(string url)
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new() { Headless = false, });
            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();
            await page.GotoAsync(url);

            await Task.Delay(5000);
            var html = await page.ContentAsync();

            return html;
        }
    }
}
