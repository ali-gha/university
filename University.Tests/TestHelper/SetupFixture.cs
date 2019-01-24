using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using University.DataLayer.Context;
using Xunit;

namespace University.Tests.TestHelper
{
    public class SetupFixture : IDisposable
    {
        private readonly CustomWebApplicationFactory _testServer;
        private readonly IServiceProvider _services;
        private readonly IServiceScope scope;
        public UniversityContext _ctx;
        public HttpClient Client { get; }
        public SetupFixture()
        {
            _testServer = new CustomWebApplicationFactory();
            Client = _testServer.CreateClient();
            scope = _testServer.Server.Host.Services.CreateScope();
            _services = scope.ServiceProvider;
            _ctx = GetService<UniversityContext>();
        }

        public T GetService<T>() => (T)_services.GetRequiredService(typeof(T));
        public void Dispose()
        {
            if (_ctx != null)
            {
                _ctx.Database.EnsureDeleted();
                _ctx.Dispose();
            }
            Client.Dispose();
            _testServer.Dispose();
        }
    }

    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<SetupFixture>
    {
    }
}