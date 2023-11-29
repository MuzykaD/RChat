using RChat.Domain.Users.DTO;
using RChat.IntegrationTests.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace RChat.IntegrationTests
{
    public class TestBase
    {
        protected HttpClient client;
        protected TestApplicationFactory factory;
        public TestBase()
        {
            factory = new();
        }

    }
}
