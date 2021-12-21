using System;

namespace ZionCodes.Core.Models.Categories.Exceptions
{
    public class NotFoundCategoryException : Exception
    {
        public NotFoundCategoryException(int categoryId)
           : base($"Couldn't find category with Id: {categoryId}.") { }
    }
}
