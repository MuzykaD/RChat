using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace RChat.IntegrationTests.Configuration
{
    internal class TestAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var context = Context;
            List<Claim> claims;
            if (context.Request.Headers.Keys.Contains("my-name"))
            {
                var name = context.Request.Headers["my-name"].First();
                var id = context.Request.Headers.Keys.Contains("my-id") ? context.Request.Headers["my-id"].First() : "";
                var email = context.Request.Headers.Keys.Contains("my-email") ? context.Request.Headers["my-id"].First() : "";
                claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, name),
                    new Claim(ClaimTypes.NameIdentifier, id),
                    new Claim(ClaimTypes.Email, email),
                };
            }
            else
            {
                claims = new List<Claim> { new Claim(ClaimTypes.Name, "Test user") };
            }
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            context.User = principal;

            var ticket = new AuthenticationTicket(principal, "Test");

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
}
