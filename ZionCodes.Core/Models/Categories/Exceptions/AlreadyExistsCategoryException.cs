using System;

namespace ZionCodes.Core.Models.Categories.Exceptions
{
    public class AlreadyExistsCategoryException : Exception
    {
        public AlreadyExistsCategoryException(Exception innerException)
            : base("Category with same id already exist", innerException)
        { }
    }
}
