using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using RChat.Domain.Users.DTO;
using RChat.Infrastructure.Context;
using RChat.Infrastructure.Contracts.UnitOfWork;
using RChat.Infrastructure.UnitOfWork;
using RChat.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.MsSql;

namespace RChat.IntegrationTests.Configuration
{
    public class TestApplicationFactory : WebApplicationFactory<Program>
    {
        //move somewhere else
        private string _connectionString = "Server=localhost\\MSSQLSERVER01;Database=RChatDb_Test;Trusted_Connection=True; TrustServerCertificate=True";
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<RChatDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddDbContext<RChatDbContext>(options =>
                {
                    options.UseSqlServer(_connectionString);
                }
                );
            });
           
        }
        public async Task<HttpClient> GetClientWithTokenAsync()
        {
            var client = CreateClient();
            var login = new LoginUserDto()
            {
                Email = "admin@mail.com",
                Password = "Admin1!"
            };
            var result = await client.PostAsJsonAsync("/api/v1/authentication/login", login);
            var token = await result.Content.ReadFromJsonAsync<UserTokenResponse>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
            return client;
        }

        public async Task<HttpClient> GetClientWithTokenAsync(LoginUserDto login)
        {
            var client = CreateClient();           
            var result = await client.PostAsJsonAsync("/api/v1/authentication/login", login);
            var token = await result.Content.ReadFromJsonAsync<UserTokenResponse>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
            return client;
        }
    }
}
