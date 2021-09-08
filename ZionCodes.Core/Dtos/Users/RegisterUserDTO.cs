using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZionCodes.Core.Dtos.Users
{
    public class RegisterUserDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
