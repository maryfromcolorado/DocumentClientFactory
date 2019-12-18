using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Documents;
using finitelogic.Common.CosmosRepositoryBase;
using finitelogic.Common.Telemetry;

namespace finitelogic.Common.DocumentClientFactory.Tests.Integration.Repository
{
    public class TestRepository : CosmosRepositoryBase<TestClass>, ICosmosRepositoryBase<TestClass>
    {
        public TestRepository(IDocumentClient client, ICosmosManager cosmosManager, ITelemetryClient telemetryClient, string databaseId, string collectionName, string partitionKey = "\\id") : base(client, cosmosManager, telemetryClient, databaseId, collectionName, partitionKey)
        {
        }
    }
}
