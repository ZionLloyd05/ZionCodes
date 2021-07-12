using System;

namespace ZionCodes.Core.Models.ReadingNotes.Exceptions
{
    public class InvalidReadingNoteException : Exception
    {
        public InvalidReadingNoteException(string parameterName, object parameterValue)
           : base($"Invalid Reading Note, " +
                  $"ParameterName: {parameterName}, " +
                  $"ParameterValue: {parameterValue}")
        { }
    }
}
