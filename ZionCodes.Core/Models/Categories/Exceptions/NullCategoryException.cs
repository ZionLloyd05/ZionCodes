using System;

namespace ZionCodes.Core.Models.Categories.Exceptions
{
    public class NullCategoryException : Exception
    {
        public NullCategoryException() :
            base("The category is null.")
        { }

    }
}
