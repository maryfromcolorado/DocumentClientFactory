using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using finitelogic.Common.Configuration;
using finitelogic.Common.CosmosRepositoryBase;
using finitelogic.Common.DocumentClientFactory;
using finitelogic.Common.DocumentClientFactory.Tests.Integration.Repository;
using finitelogic.Common.Telemetry;
using Xunit;
using System.Collections.Generic;

namespace Tests.DocumentClient
{
    public class TestDocumentClient
    {
       
        IDocumentClientFactory documentClientFactory;
        IConfiguration config;
        ITelemetryClient telemetryClient;
        TestRepository repositoryRead;
        TestRepository repositoryReadWrite;
        string databaseId;
        string collectionName = "TestCollection";
        public TestDocumentClient()
        {
            config = ConfigurationHelper.GetIConfigurationRoot(AppDomain.CurrentDomain.BaseDirectory, Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
            telemetryClient = new TelemetryClientWrap(config);
            var authKey = config.Get("TestDataSettings:AuthKey").MakeSecureString();
            databaseId = config.Get("TestDataSettings:DatabaseId");
            // need to get a few parameters for connectionpolicy
            documentClientFactory = new DocumentClientFactory(new Uri(config.Get("TestDataSettings:Endpoint")), authKey, authKey, new List<string>() { "East US" });
            var fakeDbManager = new FakeDbManager(documentClientFactory.ReadOnlyDocumentClient, databaseId);
            repositoryRead = new TestRepository(documentClientFactory.ReadOnlyDocumentClient, fakeDbManager, telemetryClient, databaseId,collectionName, @"\id");
            repositoryReadWrite = new TestRepository(documentClientFactory.ReadWriteDocumentClient, fakeDbManager, telemetryClient, databaseId, collectionName, @"\id");
        }


        [Fact]
        public async void InsertNewRecord_FollowedByFind_OnSecondary_Returns_Object()
        {
            var id = Guid.NewGuid().ToString();
            // Arrange
           
            var document = await repositoryReadWrite.AddAsync(new TestClass() { Id = id, Name = "Test123" });
            // Act
            await Task.Delay(1000);

            var queryRead = (await repositoryRead.WhereAsync(x=>x.Id == id)).ToList().FirstOrDefault();
            
            //Assert
            Assert.True(queryRead.Id == id);
        }

      

    }
}
