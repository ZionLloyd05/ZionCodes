using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZionCodes.Core.Models.ReadingNotes;
using ZionCodes.Core.Models.ReadingNotes.Exceptions;

namespace ZionCodes.Core.Services.ReadingNotes
{
    public partial class ReadingNoteService
    {
        private void ValidateReadingNoteOnCreate(ReadingNote readingNote)
        {
            ValidateReadingNoteIsNull(readingNote);
            ValidateReadingNoteIdIsNull(readingNote.Id);
            ValidateReadingNoteAuditFields(readingNote);
        }

        private void ValidateReadingNoteIsNull(ReadingNote readingNote)
        {
            if (readingNote is null)
            {
                throw new NullReadingNoteException();
            }
        }

        private void ValidateReadingNoteIdIsNull(Guid readingNoteId)
        {
            if (readingNoteId == default)
            {
                throw new InvalidReadingNoteException(
                    parameterName: nameof(ReadingNote.Id),
                    parameterValue: readingNoteId);
            }
        }

        private void ValidateReadingNoteAuditFields(ReadingNote readingNote)
        {
            switch (readingNote)
            {
                case { } when IsInvalid(input: readingNote.CreatedBy):
                    throw new InvalidReadingNoteException(
                        parameterName: nameof(readingNote.CreatedBy),
                        parameterValue: readingNote.CreatedBy);

                case { } when IsInvalid(input: readingNote.UpdatedBy):
                    throw new InvalidReadingNoteException(
                        parameterName: nameof(readingNote.UpdatedBy),
                        parameterValue: readingNote.UpdatedBy);

                case { } when IsInvalid(input: readingNote.CreatedDate):
                    throw new InvalidReadingNoteException(
                        parameterName: nameof(ReadingNote.CreatedDate),
                        parameterValue: readingNote.CreatedDate);

                default:
                    break;
            }
        }

        private bool IsInvalid(Guid input) => input == default;
        private bool IsInvalid(DateTimeOffset input) => input == default;

    }
}
