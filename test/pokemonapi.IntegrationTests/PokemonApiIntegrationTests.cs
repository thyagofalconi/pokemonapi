using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using pokemonapi.Models;

namespace pokemonapi.IntegrationTests
{
    [TestClass]
    public class PokemonApiIntegrationTests
    {
        private HttpClient _client;


        [TestInitialize]
        public void TestInitialize()
        {
            //Given

            var projectDir = GetProjectPath("", typeof(PokemonApiIntegrationTests).GetTypeInfo().Assembly);

            TestServer server = new TestServer(new WebHostBuilder()
                .UseConfiguration(new ConfigurationBuilder()
                    .SetBasePath(projectDir)
                    .AddJsonFile("appsettings.test.json")
                    .Build()
                ).UseStartup<Startup>());

            _client = server.CreateClient();
        }

        [TestMethod]
        public void GivenAValidPokemon_WhenIGetTheShakespeareanDescription_IGetAValidResponse()
        {
            // When

            var response = _client.GetAsync("/pokemon/charizard").GetAwaiter().GetResult();

            var pokemonResponse = JsonConvert.DeserializeObject<PokemonSuccessfulResponse>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());

            // Then

            Assert.IsTrue(pokemonResponse.Success == true);
            Assert.IsTrue(pokemonResponse.Name == "charizard");
            Assert.IsTrue(pokemonResponse.Description == "Charizard flies around the sky in search of powerful opponents.\nIt breathes fire of such great heat that it melts anything.\nHowever, it never turns its fiery breath on any opponent\nweaker than itself.");
        }

        [TestMethod]
        public void GivenAnInvalidPokemon_WhenIGetTheShakespeareanDescription_IGetAnErrorResponse()
        {
            // When

            var response = _client.GetAsync("/pokemon/test").GetAwaiter().GetResult();
            
            // Then

            Assert.IsTrue(response.IsSuccessStatusCode == false);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.NotFound);
        }

        private static string GetProjectPath(string projectRelativePath, Assembly startupAssembly)
        {
            // Get name of the target project which we want to test
            var projectName = startupAssembly.GetName().Name;

            // Get currently executing test project path
            var applicationBasePath = System.AppContext.BaseDirectory;

            // Find the path to the target project
            var directoryInfo = new DirectoryInfo(applicationBasePath);
            do
            {
                directoryInfo = directoryInfo.Parent;

                var projectDirectoryInfo = new DirectoryInfo(Path.Combine(directoryInfo.FullName, projectRelativePath));
                if (projectDirectoryInfo.Exists)
                {
                    var projectFileInfo = new FileInfo(Path.Combine(projectDirectoryInfo.FullName, projectName, $"{projectName}.csproj"));
                    if (projectFileInfo.Exists)
                    {
                        return Path.Combine(projectDirectoryInfo.FullName, projectName);
                    }
                }
            }
            while (directoryInfo.Parent != null);

            throw new Exception($"Project root could not be located using the application root {applicationBasePath}.");
        }
    }
}
