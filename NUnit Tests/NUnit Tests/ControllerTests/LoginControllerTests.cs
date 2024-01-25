using Azure;
using GameHeaven.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Models;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace NUnit_Tests.ControllerTests
{
    [TestFixture]
    public class LoginControllerTests
    {
        private DbContextOptions<Context> _testDbContextOptions;
        private const string BaseUrl = "http://localhost:5163/";
        private const string BearerToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6Ik1pbG9zIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZW1haWxhZGRyZXNzIjoibWV0YWxsbWVzc2FoMjJAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvc2lkIjoiMSIsImV4cCI6MTcwNjEzNzA4NCwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo0NDM1NCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NDQzNTQifQ.CVX4HeuVOMcV7sHIyIVistlsWvQr5bkK4fKOy7DOrhA";

        [SetUp]
        public void Setup()
        {
            _testDbContextOptions = new DbContextOptionsBuilder<Context>()
            .UseSqlServer("Server=tcp:game-heaven.database.windows.net,1433;Initial Catalog=GameHeavenTestiranje;Persist Security Info=False;User ID=CloudSAcc2e3ff9;Password=ZastitaInformacija2002;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;")
            .Options;
        }

        [Test]
        public async Task LoginUser_ReturnsOkObjectResult_WithValidCredentials()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);

                HttpResponseMessage response = await client.GetAsync("Login/LoginPlayer/metallmessah22@gmail.com/123");

                Assert.That(System.Net.HttpStatusCode.OK, Is.EqualTo(response.StatusCode));

            }
        }

        [Test]
        public async Task GetToken_ReturnsOkObjectResult_WithValidCredentials()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                var requestData = new
                {
                    Email = "metallmessah22@gmail.com",
                    Password = "123"
                };

                var jsonContent = JsonConvert.SerializeObject(requestData);
                var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("Login/GetToken", stringContent);

                Assert.That(System.Net.HttpStatusCode.OK, Is.EqualTo(response.StatusCode));
            }
        }

        [Test]
        public async Task GetToken_ReturnsBadRequestObjectResult_WithInvalidCredentials()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                var requestData = new
                {
                    Email = "metallmessah@gmail.com",
                    Password = "123"
                };

                var jsonContent = JsonConvert.SerializeObject(requestData);
                var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("Login/GetToken", stringContent);

                Assert.That(System.Net.HttpStatusCode.BadRequest, Is.EqualTo(response.StatusCode));
            }
        }

        [Test]  //mozda ga i obrisi
        public async Task GetToken_ReturnsBadRequestObjectResult_WithInvalidCredentials1()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                var requestData = new
                {
                    Email = "metallmessah22@gmail.com",
                    Password = "13"
                };

                var jsonContent = JsonConvert.SerializeObject(requestData);
                var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("Login/GetToken", stringContent);

                Assert.That(System.Net.HttpStatusCode.NoContent, Is.EqualTo(response.StatusCode));
            }
        }

        [Test]  
        public async Task SignupUser_ReturnsOkObjectResult_WithValidCredentials()
        {
            using (var context = new Context(_testDbContextOptions))
            {
            
                var controller = new LoginController(context,null);

                var result = await controller.SignUpUser("Pera","pera123@gmail.com","123","123") as ObjectResult;

                Assert.That(result, Is.Not.Null);
                Assert.That(result.StatusCode, Is.EqualTo(200));
            }
        }

        [Test]
        public async Task SignupUser_ReturnsBadRequestObjectResult_WithInvalidCredentials()
        {
            using (var context = new Context(_testDbContextOptions))
            {

                var controller = new LoginController(context, null);

                var result = await controller.SignUpUser("Pera", "pera123@gmail.com", "123", "123") as ObjectResult;

                Assert.That(result, Is.Not.Null);
                Assert.That(result.StatusCode, Is.EqualTo(400));
            }
        }

        [Test]
        public async Task SignupUser_ReturnsBadRequestObjectResult_WithInvalidCredentialsP()
        {
            using (var context = new Context(_testDbContextOptions))
            {

                var controller = new LoginController(context, null);

                var result = await controller.SignUpUser("Mare", "mare123@gmail.com", "13", "123") as ObjectResult;

                Assert.That(result, Is.Not.Null);
                Assert.That(result.StatusCode, Is.EqualTo(400));
            }
        }
    }
}


