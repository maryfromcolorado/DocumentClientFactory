using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Text;

namespace finitelogic.Common.DocumentClientFactory.Tests.Integration.Repository
{
    public class TestClass : Resource
    {
        public string Name { get; set; }
        public DateTime InsertedAt { get; set; } = DateTime.UtcNow;
    }
}
