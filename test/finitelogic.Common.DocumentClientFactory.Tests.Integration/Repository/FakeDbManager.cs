using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Text;
using finitelogic.Common.CosmosRepositoryBase;

namespace finitelogic.Common.DocumentClientFactory.Tests.Integration.Repository
{
    public class FakeDbManager : CosmosManager, ICosmosManager
    {
        public FakeDbManager(IDocumentClient client, string databaseId, int RUs = 400) : base(client, databaseId, RUs)
        {
        }
    }
}
