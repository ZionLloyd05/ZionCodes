using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZionCodes.Core.Models.Categories.Exceptions
{
    public class NotFoundCategoryException : Exception
    {
        public NotFoundCategoryException(Guid categoryId)
           : base($"Couldn't find category with Id: {categoryId}.") { }
    }
}
