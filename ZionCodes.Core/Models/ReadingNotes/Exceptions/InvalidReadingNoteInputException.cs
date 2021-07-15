using System;

namespace ZionCodes.Core.Models.ReadingNotes.Exceptions
{
    public class InvalidReadingNoteInputException : Exception
    {
        public InvalidReadingNoteInputException(string parameterName, object parameterValue)
              : base($"Invalid Reading note, " +
                  $"ParameterName: {parameterName}, " +
                  $"ParameterValue: {parameterValue}.")
        { }
    }
}
