using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZionCodes.Core.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string HomeRelativeUrl = "api/home";

        public async ValueTask<string> GetHomeMessage() =>
            await this.apiFactoryClient.GetContentStringAsync(HomeRelativeUrl);
    }
}
