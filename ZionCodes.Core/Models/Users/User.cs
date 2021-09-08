using System;
using System.ComponentModel.DataAnnotations;
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
