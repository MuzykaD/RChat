using RChat.IntegrationTests.DbSqlHelpers.UserDbSqlHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.IntegrationTests.Fixtures
{
    internal class AuthenticationFixture : IAsyncDisposable
    {
        private UserDbSqlHelper _userDbSqlHelper;
        public AuthenticationFixture()
        {
            _userDbSqlHelper = new UserDbSqlHelper();
        }
        public async ValueTask DisposeAsync()
        {
           
        }
    }
}
