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
    }
}
