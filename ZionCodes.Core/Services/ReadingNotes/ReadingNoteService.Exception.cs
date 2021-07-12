using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using ZionCodes.Core.Models.ReadingNotes;
using ZionCodes.Core.Models.ReadingNotes.Exceptions;

namespace ZionCodes.Core.Services.ReadingNotes
{
    public partial class ReadingNoteService
    {
        private delegate ValueTask<ReadingNote> ReturningReadingNoteFunction();

        private async ValueTask<ReadingNote> TryCatch(
            ReturningReadingNoteFunction returningReadingNoteFunction)
        {
            try
            {
                return await returningReadingNoteFunction();
            }
            catch (NullReadingNoteException nullReadingNoteException)
            {
                throw CreateAndLogValidationException(nullReadingNoteException);
            }
            catch (InvalidReadingNoteException invalidReadingNoteException)
            {
                throw CreateAndLogValidationException(invalidReadingNoteException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsReadingNoteException =
                    new AlreadyExistsReadingNoteException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsReadingNoteException);
            }
        }


        private ReadingNoteValidationException CreateAndLogValidationException(Exception exception)
        {
            var ReadingNoteValidationException = new ReadingNoteValidationException(exception);
            this.loggingBroker.LogError((Exception)ReadingNoteValidationException);

            return ReadingNoteValidationException;
        }
    }
}
