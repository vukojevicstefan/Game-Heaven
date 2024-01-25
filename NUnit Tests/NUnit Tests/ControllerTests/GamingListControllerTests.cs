using GameHeaven.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Moq;
using NUnit.Framework.Legacy;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace NUnit_Tests
{
    public class GamingListsControllerTests
    {
        private GamingListController _controller;
        private DbContextOptions<Context> _testDbContextOptions;
        private const string BaseUrl = "http://localhost:5163";
        private const string BearerToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6Ik1pbG9zIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZW1haWxhZGRyZXNzIjoibWV0YWxsbWVzc2FoMjJAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvc2lkIjoiMSIsImV4cCI6MTcwNjE0MzM0NCwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo0NDM1NCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NDQzNTQifQ.oqnDcIYamLeVVQSNlH54ur9RA89IUlLWolEleBrLB6k";

        [SetUp]
        public void Setup()
        {
            _testDbContextOptions = new DbContextOptionsBuilder<Context>()
            .UseSqlServer("Server=tcp:game-heaven.database.windows.net,1433;Initial Catalog=GameHeavenTestiranje;Persist Security Info=False;User ID=CloudSAcc2e3ff9;Password=ZastitaInformacija2002;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;")
            .Options;

            var mockContext = new Mock<Context>(_testDbContextOptions);

            // Mock the user claims
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
        public async Task DeleteGamingList_WithValidInput_ShouldReturnOkResponse()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                var userClaims = new[]
                {
            new Claim(ClaimTypes.NameIdentifier, "Milos"),
            new Claim(ClaimTypes.Email, "metallmessah22@gmail.com"),
            new Claim(ClaimTypes.Sid, "1"),
        };

                var identity = new ClaimsIdentity(userClaims, "TestAuth");
                var principal = new ClaimsPrincipal(identity);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);

                using (var context = new Context(_testDbContextOptions))
                {
                    var controller = new GamingListController(context)
                    {
                        ControllerContext = new ControllerContext
                        {
                            HttpContext = new DefaultHttpContext
                            {
                                User = principal,
                                Request =
                        {
                            Headers = { {"Authorization", "Bearer " + BearerToken} }
                        }
                            }
                        }
                    };

                    var result = await controller.DeleteGamingList("Miloseva kolekcija") as ObjectResult;

                    Assert.That(result, Is.Not.Null);
                    Assert.That(result.StatusCode, Is.EqualTo(200));
                }
            }
        }

        [Test]
        public async Task DeleteGamingList_WithInvalidInput_ShouldReturnBadRequestResponse()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                var userClaims = new[]
                {
            new Claim(ClaimTypes.NameIdentifier, "Milos"),
            new Claim(ClaimTypes.Email, "metallmessah22@gmail.com"),
            new Claim(ClaimTypes.Sid, "1"),
        };

                var identity = new ClaimsIdentity(userClaims, "TestAuth");
                var principal = new ClaimsPrincipal(identity);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);

                using (var context = new Context(_testDbContextOptions))
                {
                    var controller = new GamingListController(context)
                    {
                        ControllerContext = new ControllerContext
                        {
                            HttpContext = new DefaultHttpContext
                            {
                                User = principal,
                                Request =
                        {
                            Headers = { {"Authorization", "Bearer " + BearerToken} }
                        }
                            }
                        }
                    };

                    var result = await controller.DeleteGamingList("Kolek") as ObjectResult;

                    Assert.That(result, Is.Not.Null);
                    Assert.That(result.StatusCode, Is.EqualTo(400));
                    Assert.That(result.Value as String, Is.EqualTo("There is no game list with that name"));
                }
            }
        }

        [Test]
        public async Task DeleteGamingList_WithNoUser_ShouldReturnBadRequestResponse()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                var userClaims = new[]
                {
            new Claim(ClaimTypes.NameIdentifier, "Milos"),
            new Claim(ClaimTypes.Email, "metallmessah22@gmail.com"),
            new Claim(ClaimTypes.Sid, "1"),
        };

                var identity = new ClaimsIdentity(userClaims, "TestAuth");
                var principal = new ClaimsPrincipal(identity);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken+"1");

                using (var context = new Context(_testDbContextOptions))
                {
                    var controller = new GamingListController(context)
                    {
                        ControllerContext = new ControllerContext
                        {
                            HttpContext = new DefaultHttpContext
                            {
                                User = principal,
                                Request =
                        {
                            Headers = { {"Authorization", "Bearer " + BearerToken+"1"} }
                        }
                            }
                        }
                    };

                    var result = await controller.DeleteGamingList("Kolekcija") as ObjectResult;

                    Assert.That(result, Is.Not.Null);
                    Assert.That(result.StatusCode, Is.EqualTo(400));
                }
            }
        }

        [Test]
        public async Task CreateGamingList_WithValidInput_ShouldReturnOkResponse()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                var userClaims = new[]
                {
            new Claim(ClaimTypes.NameIdentifier, "Milos"),
            new Claim(ClaimTypes.Email, "metallmessah22@gmail.com"),
            new Claim(ClaimTypes.Sid, "1"),
        };

                var identity = new ClaimsIdentity(userClaims, "TestAuth");
                var principal = new ClaimsPrincipal(identity);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);

                using (var context = new Context(_testDbContextOptions))
                {
                    var controller = new GamingListController(context)
                    {
                        ControllerContext = new ControllerContext
                        {
                            HttpContext = new DefaultHttpContext
                            {
                                User = principal,
                                Request =
                        {
                            Headers = { {"Authorization", "Bearer " + BearerToken} }
                        }
                            }
                        }
                    };

                    var result = await controller.CreateNewGamingList("Favorites") as ObjectResult;

                    Assert.That(result, Is.Not.Null);
                    Assert.That(result.StatusCode, Is.EqualTo(200));
                }
            }
        }

        [Test]
        public async Task CreateGamingList_WithInvalidInput_ShouldReturnBadRequestResponse()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                var userClaims = new[]
                {
            new Claim(ClaimTypes.NameIdentifier, "Milos"),
            new Claim(ClaimTypes.Email, "metallmessah22@gmail.com"),
            new Claim(ClaimTypes.Sid, "1"),
        };

                var identity = new ClaimsIdentity(userClaims, "TestAuth");
                var principal = new ClaimsPrincipal(identity);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);

                using (var context = new Context(_testDbContextOptions))
                {
                    var controller = new GamingListController(context)
                    {
                        ControllerContext = new ControllerContext
                        {
                            HttpContext = new DefaultHttpContext
                            {
                                User = principal,
                                Request =
                        {
                            Headers = { {"Authorization", "Bearer " + BearerToken} }
                        }
                            }
                        }
                    };

                    var result = await controller.CreateNewGamingList("Miloseva kolekcija") as ObjectResult;

                    Assert.That(result, Is.Not.Null);
                    Assert.That(result.StatusCode, Is.EqualTo(400));
                }
            }
        }

        [Test]
        public async Task CreateGamingList_WithInvalidUser_ShouldReturnBadRequestResponse()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                var userClaims = new[]
                {
            new Claim(ClaimTypes.NameIdentifier, "Milos"),
            new Claim(ClaimTypes.Email, "metallmessah22@gmail.com"),
            new Claim(ClaimTypes.Sid, "1"),
        };

                var identity = new ClaimsIdentity(userClaims, "TestAuth");
                var principal = new ClaimsPrincipal(identity);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken+"1");

                using (var context = new Context(_testDbContextOptions))
                {
                    var controller = new GamingListController(context)
                    {
                        ControllerContext = new ControllerContext
                        {
                            HttpContext = new DefaultHttpContext
                            {
                                User = principal,
                                Request =
                        {
                            Headers = { {"Authorization", "Bearer " + BearerToken+"1"} }
                        }
                            }
                        }
                    };

                    var result = await controller.CreateNewGamingList("Miloseva kolekcija") as ObjectResult;

                    Assert.That(result, Is.Not.Null);
                    Assert.That(result.StatusCode, Is.EqualTo(400));
                }
            }
        }

        [Test]
        public async Task AddGameGamingList_WithValidInput_ShouldReturnOkResponse()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                var userClaims = new[]
                {
            new Claim(ClaimTypes.NameIdentifier, "Milos"),
            new Claim(ClaimTypes.Email, "metallmessah22@gmail.com"),
            new Claim(ClaimTypes.Sid, "1"),
        };

                var identity = new ClaimsIdentity(userClaims, "TestAuth");
                var principal = new ClaimsPrincipal(identity);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);

                using (var context = new Context(_testDbContextOptions))
                {
                    var controller = new GamingListController(context)
                    {
                        ControllerContext = new ControllerContext
                        {
                            HttpContext = new DefaultHttpContext
                            {
                                User = principal,
                                Request =
                        {
                            Headers = { {"Authorization", "Bearer " + BearerToken} }
                        }
                            }
                        }
                    };

                    var result = await controller.AddGameToGamingList("Valorant","Favorites") as ObjectResult;

                    Assert.That(result, Is.Not.Null);
                    Assert.That(result.StatusCode, Is.EqualTo(200));
                }
            }
        }


        [Test]
        public async Task AddGameGamingList_WithInvalidInput_ShouldReturnBadRequestResponse()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                var userClaims = new[]
                {
            new Claim(ClaimTypes.NameIdentifier, "Milos"),
            new Claim(ClaimTypes.Email, "metallmessah22@gmail.com"),
            new Claim(ClaimTypes.Sid, "1"),
        };

                var identity = new ClaimsIdentity(userClaims, "TestAuth");
                var principal = new ClaimsPrincipal(identity);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);

                using (var context = new Context(_testDbContextOptions))
                {
                    var controller = new GamingListController(context)
                    {
                        ControllerContext = new ControllerContext
                        {
                            HttpContext = new DefaultHttpContext
                            {
                                User = principal,
                                Request =
                        {
                            Headers = { {"Authorization", "Bearer " + BearerToken} }
                        }
                            }
                        }
                    };

                    var result = await controller.AddGameToGamingList("Vaorant", "Favorites") as ObjectResult;

                    Assert.That(result, Is.Not.Null);
                    Assert.That(result.StatusCode, Is.EqualTo(400));
                }
            }
        }

        [Test]
        public async Task AddGameGamingList_WithInvalidInput_ShouldReturnBadRequestResponse1()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                var userClaims = new[]
                {
            new Claim(ClaimTypes.NameIdentifier, "Milos"),
            new Claim(ClaimTypes.Email, "metallmessah22@gmail.com"),
            new Claim(ClaimTypes.Sid, "1"),
        };

                var identity = new ClaimsIdentity(userClaims, "TestAuth");
                var principal = new ClaimsPrincipal(identity);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);

                using (var context = new Context(_testDbContextOptions))
                {
                    var controller = new GamingListController(context)
                    {
                        ControllerContext = new ControllerContext
                        {
                            HttpContext = new DefaultHttpContext
                            {
                                User = principal,
                                Request =
                        {
                            Headers = { {"Authorization", "Bearer " + BearerToken} }
                        }
                            }
                        }
                    };

                    var result = await controller.AddGameToGamingList("Valorant", "Favoraites") as ObjectResult;

                    Assert.That(result, Is.Not.Null);
                    Assert.That(result.StatusCode, Is.EqualTo(400));
                }
            }
        }

        [Test]
        public async Task GetGamingListsWithGames_WithValidInput_ShouldReturnOkResponse()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                var userClaims = new[]
                {
            new Claim(ClaimTypes.NameIdentifier, "Milos"),
            new Claim(ClaimTypes.Email, "metallmessah22@gmail.com"),
            new Claim(ClaimTypes.Sid, "1"),
        };

                var identity = new ClaimsIdentity(userClaims, "TestAuth");
                var principal = new ClaimsPrincipal(identity);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);

                using (var context = new Context(_testDbContextOptions))
                {
                    var controller = new GamingListController(context)
                    {
                        ControllerContext = new ControllerContext
                        {
                            HttpContext = new DefaultHttpContext
                            {
                                User = principal,
                                Request =
                        {
                            Headers = { {"Authorization", "Bearer " + BearerToken} }
                        }
                            }
                        }
                    };

                    var result = await controller.GetGamingListsWithGames() as ObjectResult;

                    Assert.That(result, Is.Not.Null);
                    Assert.That(result.StatusCode, Is.EqualTo(200));
                }
            }
        }
    }
}