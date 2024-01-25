using GameHeaven.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Moq;
using System.Security.Claims;

namespace NUnit_Tests
{
    [TestFixture]
    public class GameControllerTests
    {
        private GamingListController _controller;
        private DbContextOptions<Context> _testDbContextOptions;

        [SetUp]
        public void Setup()
        {
            _testDbContextOptions = new DbContextOptionsBuilder<Context>()
            .UseSqlServer("Server=tcp:game-heaven.database.windows.net,1433;Initial Catalog=GameHeavenTestiranje;Persist Security Info=False;User ID=CloudSAcc2e3ff9;Password=ZastitaInformacija2002;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;")
            .Options;

            var mockContext = new Mock<Context>(_testDbContextOptions);

            var userClaims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, "Milos"),
            new Claim(ClaimTypes.Email, "metallmessah22@gmail.com"),
            new Claim(ClaimTypes.Sid, "123"),
        };

            var identity = new ClaimsIdentity(userClaims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            _controller = new GamingListController(mockContext.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = principal,
                        Request =
                    {
                        Headers = { {"Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6Ik1pbG9zIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZW1haWxhZGRyZXNzIjoibWV0YWxsbWVzc2FoMjJAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvc2lkIjoiMSIsImV4cCI6MTcwNjEyNjg4MCwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo0NDM1NCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NDQzNTQifQ.d_oMottC--j3w29Rcpq9QdXBr4JZrjFAwPHkKjp_pXI" } }
                    }
                    },
                },
            };
        }


        [Test]
        public async Task GetGames_ReturnsOkResultWithData()
        {
            using (var context = new Context(_testDbContextOptions))
            {
                var controller = new GameController(context);

                var result = await controller.GetGames() as ObjectResult;

                Assert.That(result, Is.Not.Null);
                Assert.That(result.StatusCode, Is.EqualTo(200));

                var gamesWithStrings = result.Value as IEnumerable<object>;
                Assert.That(gamesWithStrings, Is.Not.Null);

                Assert.That(gamesWithStrings.Count(), Is.EqualTo(2));
            }
        }

        [Test]
        public async Task GetGames_ReturnsErrorBecauseThereAreGames()
        {
            using (var context = new Context(_testDbContextOptions))
            {
                var controller = new GameController(context);

                var result = await controller.GetGames() as ObjectResult;

                Assert.That(result, Is.Not.Null);
                Assert.That(result.StatusCode, Is.EqualTo(404));

                var errorMessage = result.Value as string;
                Assert.That(errorMessage, Is.EqualTo("There are still no games in the database"));
            }
        }

        [Test]
        public async Task GetGames_ReturnsErrorWithGamesInDatabase()
        {
            using (var context = new Context(_testDbContextOptions))
            {
                var controller = new GameController(context);
                var result = await controller.GetGames() as ObjectResult;

                Assert.That(result, Is.Not.Null);
                Assert.That(result.StatusCode, Is.EqualTo(400));

                var errorMessage = result.Value as string;
                Assert.That(errorMessage, Is.EqualTo("Error with getting games from the database"));
            }
        }


        [Test]
        public async Task CreateGame_ReturnsOkResultForSuccessfulCreation()
        {
            var newGame = new Game
            {
                Title = "Valorant",
                Genre = Genre.Action,
                Platform = Platform.PC,
                Description = "FPS",
                Image = "images/Valorant.jpg"
            };

            using (var context = new Context(_testDbContextOptions))
            {
                var controller = new GameController(context);

                var result = await controller.CreateGame(newGame) as ObjectResult;

                Assert.That(result, Is.Not.Null);
                Assert.That(result.StatusCode, Is.EqualTo(200));

                var successMessage = result.Value as string;
                Assert.That(successMessage, Is.EqualTo("Game added successfully!"));
            }
        }

        [Test]
        public async Task CreateGame_ReturnsBadRequestOnException()
        {
            // Arrange
            var newGame = new Game
            {
                Title = "Valorant1",
                Genre = Genre.Action,
                Platform = Platform.PC,
                Description = "FPS",
                Image = "images/Valorant.jpg"
            };

            var mockContext = new Mock<Context>(_testDbContextOptions);
            mockContext.Setup(c => c.SaveChangesAsync(default))
                       .ThrowsAsync(new DbUpdateException("Intentional error", new Exception()));


            var controller = new GameController(mockContext.Object);

            var result = await controller.CreateGame(newGame) as ObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(400));

            var errorMessage = result.Value as string;
            Assert.That(errorMessage, Is.Not.Null);

        }

        [Test]
        public async Task CreateGame_ReturnsBadRequestForInvalidModelState()
        {
            var invalidGame = new Game
            {
                // Missing required properties intentionally
            };
            using (var context = new Context(_testDbContextOptions))
            {
                var controller = new GameController(context);

                var result = await controller.CreateGame(invalidGame) as BadRequestObjectResult;

                Assert.That(result, Is.Not.Null);
                Assert.That(result.StatusCode, Is.EqualTo(400));

                var errorMessage = result.Value as string;
                Assert.That(errorMessage, Is.EqualTo("You have to insert the name of the game"));
            }
        }
    }
}
