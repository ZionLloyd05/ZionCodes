﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZionCodes.Core.Models.Users;

namespace ZionCodes.Core.Services.Users
{
    public interface IUserService
    {
        ValueTask<User> RegisterUserAsync(User user, string password);
        ValueTask<User> RemoveUserByIdAsync(Guid userId);
        ValueTask<User> RetrieveUserByIdAsync(Guid userId);
        ICollection<User> RetrieveAllUsers();
        ValueTask<User> ModifyUserAsync(User course);
    }
}
