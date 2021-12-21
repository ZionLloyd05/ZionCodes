using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZionCodes.Core.Brokers.DateTimes;
using ZionCodes.Core.Brokers.Loggings;
using ZionCodes.Core.Models.Users;
using ZionCodes.Web.Api.Brokers.UserManagement;

namespace ZionCodes.Core.Services.Users
{
    public partial class UserService : IUserService
    {
        private readonly IUserManagementBroker userManagementBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public UserService(IUserManagementBroker userManagementBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.userManagementBroker = userManagementBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<User> RemoveUserByIdAsync(Guid userId) =>
        TryCatch(async () =>
        {
            ValidateUserIdIsNull(userId);

            User maybeUser =
               await this.userManagementBroker.SelectUserByIdAsync(userId);

            ValidateStorageUser(maybeUser, userId);

            return await this.userManagementBroker.DeleteUserAsync(maybeUser);
        });

        public ValueTask<User> ModifyUserAsync(User user) =>
        TryCatch(async () =>
        {
            ValidateUserOnModify(user);

            User maybeUser = await this.userManagementBroker.SelectUserByIdAsync(user.Id);

            ValidateStorageUser(maybeUser, user.Id);
            ValidateAgainstStorageUserOnModify(inputUser: user, storageUser: maybeUser);

            return await this.userManagementBroker.UpdateUserAsync(user);
        });

        public ValueTask<User> RegisterUserAsync(User user, string password) =>
        TryCatch(async () =>
        {
            ValidateUserOnCreate(user, password);

            return await this.userManagementBroker.InsertUserAsync(user, password);
        });

        public ICollection<User> RetrieveAllUsers() =>
        TryCatch(() =>
        {
            ICollection<User> storageUsers = this.userManagementBroker.SelectAllUsers();
            ValidateStorageUsers(storageUsers);

            return storageUsers;
        });

        public ValueTask<User> RetrieveUserByIdAsync(Guid userId) =>
        TryCatch(async () =>
        {
            ValidateUserIdIsNull(userId);
            User storageUser = await this.userManagementBroker.SelectUserByIdAsync(userId);
            ValidateStorageUser(storageUser, userId);

            return storageUser;
        });
    }
}
