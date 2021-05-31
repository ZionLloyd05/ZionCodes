using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZionCodes.Core.Models.Categories.Exceptions
{
    public class AlreadyExistsCategoryException : Exception
    {
        public AlreadyExistsCategoryException(Exception innerException)
            : base("Category with same id already exist", innerException)
        {  }
    }
}
