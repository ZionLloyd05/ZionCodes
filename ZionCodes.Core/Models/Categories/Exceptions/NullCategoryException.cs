using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZionCodes.Core.Models.Categories.Exceptions
{
    public class NullCategoryException : Exception
    {
        public NullCategoryException() :
            base("The category is null.")
        { }

    }
}
