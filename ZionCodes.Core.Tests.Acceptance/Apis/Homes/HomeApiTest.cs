using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using ZionCodes.Core.Tests.Acceptance.Brokers;

namespace ZionCodes.Core.Tests.Acceptance.Apis.Homes
{
    [Collection(nameof(ApiTestCollection))]
    public class HomeApiTest
    {
        private readonly ApiBroker apiBroker;

        public HomeApiTest(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        [Fact]
        public async Task ShouldReturnHomeMessageAsync()
        {
            // given
            string expectedMessage =
                "Hello Luigi, Mario has taken the princess to another castle!";

            // when
            string actualMessage =
                await apiBroker.GetHomeMessage();

            // then
            actualMessage.Should().Be(expectedMessage);
        }
    }
}
