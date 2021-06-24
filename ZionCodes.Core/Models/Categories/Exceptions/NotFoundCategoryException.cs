using System;

namespace ZionCodes.Core.Models.Categories.Exceptions
{
    public class NotFoundCategoryException : Exception
    {
        public NotFoundCategoryException(Guid categoryId)
           : base($"Couldn't find category with Id: {categoryId}.") { }
    }
}
