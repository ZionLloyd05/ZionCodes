using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ZionCodes.Core.Tests.Acceptance.Brokers
{
    [CollectionDefinition(nameof(ApiTestCollection))]
    public class ApiTestCollection : ICollectionFixture<ApiBroker>
    {
    }
}
