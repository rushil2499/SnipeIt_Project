using Microsoft.Playwright;

namespace SnipeIt.Tests.Pages;

public class LoginPage : BasePage
{
    private readonly ILocator _usernameInput;
    private readonly ILocator _passwordInput;
    private readonly ILocator _loginButton;

    public LoginPage(IPage page) : base(page)
    {
        _usernameInput = Page.Locator("input[name='username']");
        _passwordInput = Page.Locator("input[name='password']");
        _loginButton = Page.Locator("button:has-text('Login')");
    }

    public async Task NavigateAsync(string url = "https://demo.snipeitapp.com/login")
    {
        await Page.GotoAsync(url);
    }

    public async Task LoginAsync(string username = "admin", string password = "password")
    {
        await _usernameInput.FillAsync(username);
        await _passwordInput.FillAsync(password);
        await _loginButton.ClickAsync();

        // Ensure we are logged in and off the login page
        await Page.WaitForURLAsync(url => !url.Contains("/login"), new() { Timeout = 15000 });
    }
}