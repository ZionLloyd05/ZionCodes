using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZionCodes.Core.Models.Tags;
using ZionCodes.Core.Models.Tags.Exceptions;

namespace ZionCodes.Core.Services.Tags
{
    public partial class TagService
    {
        private delegate ValueTask<Tag> ReturningTagFunction();

        private async ValueTask<Tag> TryCatch(
            ReturningTagFunction returningTagFunction)
        {
            try
            {
                return await returningTagFunction();
            }
            catch (NullTagException nullTagException)
            {
                throw CreateAndLogValidationException(nullTagException);
            }
        }


        private TagValidationException CreateAndLogValidationException(Exception exception)
        {
            var TagValidationException = new TagValidationException(exception);
            this.loggingBroker.LogError((Exception)TagValidationException);

            return TagValidationException;
        }
    }
}
