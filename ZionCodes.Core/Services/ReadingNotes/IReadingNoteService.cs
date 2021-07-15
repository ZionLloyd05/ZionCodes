﻿using System.Threading.Tasks;
using ZionCodes.Core.Models.ReadingNotes;

namespace ZionCodes.Core.Services.ReadingNotes
{
    public interface IReadingNoteService
    {
        ValueTask<ReadingNote> AddReadingNoteAsync(ReadingNote readingNote);
    }
}
