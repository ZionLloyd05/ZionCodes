using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZionCodes.Core.Models.Categories.Exceptions
{
    public class LockedCategoryException : Exception
    {
        public LockedCategoryException(Exception innerException)
           : base("Locked category record exception, please try again later.", innerException) { }
    }
}
