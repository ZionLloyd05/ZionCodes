using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ZionCodes.Core.Models.Users
{
    public class User : IdentityUser<Guid>
    {
        [MaxLength(40)]
        public override Guid Id
        {
            get => base.Id;
            set => base.Id = value;
        }

        public string Name { get; set; }
        public UserStatus Status { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
