using System;
using System.Linq;
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

        private void ValidateReadingNoteId(Guid readingNoteId)
        {
            if (readingNoteId == Guid.Empty)
            {
                throw new InvalidReadingNoteInputException(
                    parameterName: nameof(ReadingNote.Id),
                    parameterValue: readingNoteId);
            }
        }

        private void ValidateReadingNoteOnModify(ReadingNote readingNote)
        {
            ValidateReadingNoteIsNull(readingNote);
            ValidateReadingNoteAuditFieldsOnModify(readingNote);
        }
        private void ValidateReadingNoteAuditFieldsOnModify(ReadingNote readingNote)
        {
            switch (readingNote)
            {
                case { } when IsInvalid(input: readingNote.UpdatedDate):
                    throw new InvalidReadingNoteException(
                        parameterName: nameof(readingNote.UpdatedDate),
                        parameterValue: readingNote.UpdatedDate);

            }
        }
        private void ValidateStorageReadingNotes(IQueryable<ReadingNote> storageCategories)
        {
            if (storageCategories.Count() == 0)
            {
                this.loggingBroker.LogWarning("No reading notes found in storage.");
            }
        }


        private void ValidateStorageReadingNote(ReadingNote storageReadingNote, Guid readingNoteId)
        {
            if (storageReadingNote == null)
            {
                throw new NotFoundReadingNoteException(readingNoteId);
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

                case { } when IsInvalid(input: readingNote.UpdatedDate):
                    throw new InvalidReadingNoteException(
                        parameterName: nameof(readingNote.UpdatedDate),
                        parameterValue: readingNote.UpdatedDate);

                case { } when readingNote.UpdatedDate != readingNote.CreatedDate:
                    throw new InvalidReadingNoteException(
                        parameterName: nameof(readingNote.UpdatedDate),
                        parameterValue: readingNote.UpdatedDate);

                case { } when IsDateNotRecent(readingNote.CreatedDate):
                    throw new InvalidReadingNoteException(
                        parameterName: nameof(ReadingNote.CreatedDate),
                        parameterValue: readingNote.CreatedDate);
                default:
                    break;
            }
        }

        private bool IsInvalid(Guid input) => input == default;
        private bool IsInvalid(DateTimeOffset input) => input == default;
        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
        }
    }
}
