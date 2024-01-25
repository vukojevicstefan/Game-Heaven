using System.Net.Http.Headers;
using System.Security.Claims;
using GameHeaven.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Moq;

namespace NUnit_Tests
{
    [TestFixture]
    public class PlayerControllerTests
    {
        private PlayerController _controller;
        private DbContextOptions<Context> _testDbContextOptions;
        private Claim[] userClaims;
        private ClaimsIdentity identity;
        private ClaimsPrincipal principal;
        private const string BaseUrl = "http://localhost:5163/";
        private const string BearerToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6Ik1pbG9zIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZW1haWxhZGRyZXNzIjoibWV0YWxsbWVzc2FoMjJAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvc2lkIjoiMSIsImV4cCI6MTcwNjEzNzA4NCwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo0NDM1NCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NDQzNTQifQ.CVX4HeuVOMcV7sHIyIVistlsWvQr5bkK4fKOy7DOrhA";


        [SetUp]
        public void Setup()
        {
            _testDbContextOptions = new DbContextOptionsBuilder<Context>()
            .UseSqlServer("Server=tcp:game-heaven.database.windows.net,1433;Initial Catalog=GameHeavenTestiranje;Persist Security Info=False;User ID=CloudSAcc2e3ff9;Password=ZastitaInformacija2002;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;")
            .Options;

            var mockContext = new Mock<Context>(_testDbContextOptions);

            _controller = new PlayerController(mockContext.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = principal,
                        Request =
                    {
                        Headers = { {"Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6Ik1pbG9zIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZW1haWxhZGRyZXNzIjoibWV0YWxsbWVzc2FoMjJAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvc2lkIjoiMSIsImV4cCI6MTcwNjEzNzA4NCwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo0NDM1NCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NDQzNTQifQ.CVX4HeuVOMcV7sHIyIVistlsWvQr5bkK4fKOy7DOrhA" } }
                    }
                    },
                },
            };
        }

        [Test]
        public async Task GetCurrentUserData_ReturnsBadRequestWhenNotAuthenticated()
        {
            using (var context = new Context(_testDbContextOptions))
            {
                var controller = new PlayerController(context);

                var result = await controller.GetCurrentUserData() as ObjectResult;

                Assert.That(result, Is.Not.Null);
                Assert.That(result.StatusCode, Is.EqualTo(400));

                var errorMessage = result.Value as string;
                Console.WriteLine(errorMessage);
                Assert.That(errorMessage, Is.EqualTo("User is null"));
            }
        }


        [Test]
        public async Task GetCurrentUserData_ReturnsOkObjectResult_WithValidPlayer()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);

                HttpResponseMessage response = await client.GetAsync("Player/GetCurrentUserData");

                Assert.That(System.Net.HttpStatusCode.OK, Is.EqualTo(response.StatusCode));
            }
        }

    }
}
