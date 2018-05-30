using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using crossblog.tests.Integration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace crossblog.tests
{
    public abstract class BaseClientFixtureTest : IDisposable
    {

        protected readonly TestServer _server;
        protected readonly HttpClient _client;

        protected BaseClientFixtureTest()
        {
            _server = new TestServer(WebHost.CreateDefaultBuilder().UseStartup<Startup>());
            _client = _server.CreateClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void Dispose()
        {
            _client.Dispose();
            _server.Dispose();
        }

    }
}
