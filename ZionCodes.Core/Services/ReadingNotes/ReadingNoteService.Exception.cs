using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ZionCodes.Core.Models.ReadingNotes;
using ZionCodes.Core.Models.ReadingNotes.Exceptions;

namespace ZionCodes.Core.Services.ReadingNotes
{
    public partial class ReadingNoteService
    {
        private delegate ValueTask<ReadingNote> ReturningReadingNoteFunction();
        private delegate ICollection<ReadingNote> ReturningCollectionReadingNoteFunction();


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
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedReadingNoteException = new LockedReadingNoteException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedReadingNoteException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw CreateAndLogDependencyException(dbUpdateException);
            }
            catch (InvalidReadingNoteInputException invalidReadingNoteInputException)
            {
                throw CreateAndLogValidationException(invalidReadingNoteInputException);
            }
            catch (NotFoundReadingNoteException notFoundReadingNoteException)
            {
                throw CreateAndLogValidationException(notFoundReadingNoteException);
            }
            catch (Exception exception)
            {
                throw CreateAndLogServiceException(exception);
            }
        }

        private ICollection<ReadingNote> TryCatch
          (ReturningCollectionReadingNoteFunction ReturningCollectionReadingNoteFunction)
        {
            try
            {
                return ReturningCollectionReadingNoteFunction();
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (Exception exception)
            {
                throw CreateAndLogServiceException(exception);
            }
        }

        private ReadingNoteValidationException CreateAndLogValidationException(Exception exception)
        {
            var ReadingNoteValidationException = new ReadingNoteValidationException(exception);
            this.loggingBroker.LogError((Exception)ReadingNoteValidationException);

            return ReadingNoteValidationException;
        }

        private ReadingNoteDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var readingNoteDependencyException = new ReadingNoteDependencyException(exception);
            this.loggingBroker.LogCritical(readingNoteDependencyException);

            return readingNoteDependencyException;
        }

        private ReadingNoteDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var readingNoteDependencyException = new ReadingNoteDependencyException(exception);
            this.loggingBroker.LogError(readingNoteDependencyException);

            return readingNoteDependencyException;
        }

        private ReadingNoteServiceException CreateAndLogServiceException(Exception exception)
        {
            var readingNoteServiceException = new ReadingNoteServiceException(exception);
            this.loggingBroker.LogError(readingNoteServiceException);

            return readingNoteServiceException;
        }
    }
}
