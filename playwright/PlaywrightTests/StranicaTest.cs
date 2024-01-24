using System.Reflection.Metadata;

namespace PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Tests : PageTest
{
    IPage page;
    IBrowser browser;

    [SetUp]
    public async Task Setup()
    {
        browser = await Playwright.Chromium.LaunchAsync(new()
        {
            Headless = false,
            SlowMo = 1000
        });

        page = await browser.NewPageAsync(new()
        {
            ViewportSize = new()
            {
                Width = 1280,
                Height = 720
            },
            ScreenSize = new()
            {
                Width = 1280,
                Height = 720
            },
/*            RecordVideoSize = new()
            {
                Width = 1280,
                Height = 720
            },
            RecordVideoDir = "../../../Videos"*/
        });
    }
    [Test]
    public async Task VisitHomePage()
    {
        await page.GotoAsync("http://localhost:3000/");

        // Assuming you have a function like `Expect` to handle assertions
        await Expect(page).ToHaveTitleAsync("Game Heaven");

        // Wait for 5 seconds
        await Task.Delay(3000);

        Assert.AreEqual("http://localhost:3000/login-register", page.Url);
    }

    [Test]
    public async Task LogInAndLogOut()
    {
        await page.GotoAsync("http://localhost:3000/login-register");

        // Assuming you have a function like `Expect` to handle assertions
        await Expect(page).ToHaveTitleAsync("Game Heaven");

        // Additional Playwright code for interacting with the login form, filling in details, and submitting
        await page.FillAsync("input[name=loginEmail]", "stefan.s.vukojevic@gmail.com");
        await page.FillAsync("input[name=loginPassword]", "1");
        await page.ClickAsync("button[type=submit]");

        // Add any assertions or checks for successful 

        Assert.AreEqual("http://localhost:3000/", page.Url);

        await page.ClickAsync("text=Log out");

        await Task.Delay(1000);

        Assert.AreEqual("http://localhost:3000/login-register", page.Url);
    }
    [Test]
    public async Task Register()
    {
        await page.GotoAsync("http://localhost:3000/login-register");

        await Expect(page).ToHaveTitleAsync("Game Heaven");

        await page.FillAsync("input[name=registerUsername]", "username");
        await page.FillAsync("input[name=registerEmail]", "email@gmail.com");
        await page.FillAsync("input[name=registerPassword]", "password");
        await page.ClickAsync("button[name=button-register]");

        Assert.AreEqual("http://localhost:3000/login-register", page.Url);
    }
    [Test]
    public async Task FilterGamesByNameAndRating()
    {
        await page.GotoAsync("http://localhost:3000/login-register");

        // Assuming you have a function like `Expect` to handle assertions
        await Expect(page).ToHaveTitleAsync("Game Heaven");

        // Additional Playwright code for interacting with the login form, filling in details, and submitting
        await page.FillAsync("input[name=loginEmail]", "stefan.s.vukojevic@gmail.com");
        await page.FillAsync("input[name=loginPassword]", "1");
        await page.ClickAsync("button[type=submit]");

        // Filter games by name "Prva" and rating 1
        await page.TypeAsync("input[name=title]", "Prva");
        await page.TypeAsync("input[name=minRating]", "1");
        await page.TypeAsync("input[name=platform]", "PC");
        await page.TypeAsync("input[name=genre]", "Adventure");

        // Click the "Apply" button
        await page.ClickAsync("button[name=Apply]");

        // Wait for the filtered games to be rendered
        await page.WaitForSelectorAsync(".cards");

        await Expect(page.Locator(".game-name")).ToContainTextAsync("Prva");
    }
    [Test]
    public async Task CreateCollection()
    {
        await page.GotoAsync("http://localhost:3000/login-register");

        // Assuming you have a function like `Expect` to handle assertions
        await Expect(page).ToHaveTitleAsync("Game Heaven");
        
        //Log In information
        await page.FillAsync("input[name=loginEmail]", "stefan.s.vukojevic@gmail.com");
        await page.FillAsync("input[name=loginPassword]", "1");
        await page.ClickAsync("button[type=submit]");

        await page.GotoAsync("http://localhost:3000/my-games");

        await page.ClickAsync("button[name=AddCollection]");

        await Expect(page.Locator(".modal-content h2")).ToContainTextAsync("Create a New Collection");

        await page.TypeAsync("input[name=CollectionNameInput]", "Kolekcija2");

        await page.ClickAsync("button[name=ButtonCreate]");

        await Task.Delay(1000);

        await Expect(page.Locator("h1[name=Kolekcija2]")).ToContainTextAsync("Kolekcija2");
    }
    [Test]
    public async Task DeleteGamesCollection()
    {
        await page.GotoAsync("http://localhost:3000/login-register");

        // Assuming you have a function like `Expect` to handle assertions
        await Expect(page).ToHaveTitleAsync("Game Heaven");

        //Log In information
        await page.FillAsync("input[name=loginEmail]", "stefan.s.vukojevic@gmail.com");
        await page.FillAsync("input[name=loginPassword]", "1");
        await page.ClickAsync("button[type=submit]");

        await page.GotoAsync("http://localhost:3000/my-games");

        await page.ClickAsync("h1[name=Kolekcija2] button");

        await Task.Delay(1000);

        var divKolekcija2 = await page.QuerySelectorAsync("h1[name=Kolekcija2]");
        Assert.Null(divKolekcija2);
    }
    [Test]
    public async Task AddGameToCollection()
    {
        await page.GotoAsync("http://localhost:3000/login-register");

        // Assuming you have a function like `Expect` to handle assertions
        await Expect(page).ToHaveTitleAsync("Game Heaven");

        //Log In information
        await page.FillAsync("input[name=loginEmail]", "stefan.s.vukojevic@gmail.com");
        await page.FillAsync("input[name=loginPassword]", "1");
        await page.ClickAsync("button[type=submit]");

        await Task.Delay(1000);

        await page.ClickAsync(".game-card[name=Prva]");

        await Expect(page.Locator("h2")).ToContainTextAsync("Prva");

        await page.SelectOptionAsync("select[name=select-collection]", "Kolekcija2");

        await page.ClickAsync("button[name=buttonAddGameToCollection]");

        await Task.Delay(1000);
    }
    [Test]
    public async Task SubmitReview()
    {
        await page.GotoAsync("http://localhost:3000/login-register");

        // Assuming you have a function like `Expect` to handle assertions
        await Expect(page).ToHaveTitleAsync("Game Heaven");

        //Log In information
        await page.FillAsync("input[name=loginEmail]", "stefan.s.vukojevic@gmail.com");
        await page.FillAsync("input[name=loginPassword]", "1");
        await page.ClickAsync("button[type=submit]");

        await Task.Delay(1000);

        await page.ClickAsync(".game-card[name=Valorant]");

        await Expect(page.Locator("h2")).ToContainTextAsync("Valorant");

        await page.TypeAsync("input[name=my-rating]", "5");
        
        await page.Keyboard.PressAsync("Tab");

        await page.Keyboard.TypeAsync("Amazing");

        await page.ClickAsync("button[name=submit-review]");
    }
    [TearDown]
    public async Task Teardown()
    {
        await page.CloseAsync();
        await browser.DisposeAsync();
    }
}
