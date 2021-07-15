using System;

namespace ZionCodes.Core.Models.ReadingNotes.Exceptions
{
    public class ReadingNoteDependencyException : Exception
    {
        public ReadingNoteDependencyException(Exception innerException)
           : base("Service dependency error occurred, category support.", innerException) { }
    }
}
