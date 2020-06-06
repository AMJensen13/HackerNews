using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace HackerNews.Tests
{
    [TestClass]
    public class StoryIntegrationTests
    {
        private HttpClient _client;

        [TestInitialize]
        public async Task InitializeTests()
        {
            var projectDir = Directory.GetCurrentDirectory();
            var configPath = Path.Combine(projectDir, "appsettings.json");

            var hostBuilder = new HostBuilder()
                            .ConfigureWebHost(webHost =>
                            {
                                // Add TestServer
                                webHost.UseTestServer();
                                webHost.UseStartup<Startup>();
                                webHost.ConfigureAppConfiguration((context, config) =>
                                {
                                    config.AddJsonFile(configPath);
                                });
                                // Specify the environment
                                webHost.UseEnvironment("Development");
                            });

            var host = await hostBuilder.StartAsync();

            _client = host.GetTestClient();
        }

        [TestMethod]
        public async Task CanGetNewsItems()
        {
            var response = await _client.GetAsync("/News?count=10");

            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }
}
