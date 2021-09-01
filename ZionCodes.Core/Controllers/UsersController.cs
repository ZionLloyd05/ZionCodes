using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZionCodes.Core.Models.Users;
using ZionCodes.Core.Services.Users;

namespace ZionCodes.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController<User>
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        public ValueTask<ActionResult<User>> PostUserAsync(User user, string password = "Test@123") =>
       TryCatchUserFunction(async () =>
       {
           User persistedUser =
                   await this.userService.RegisterUserAsync(user, password);

           return Ok(persistedUser);
       });

        [HttpGet]
        public ActionResult<IQueryable<User>> GetAllUsers() =>
        TryCatchUserFunction(() =>
        {
            IQueryable storageUser =
                    this.userService.RetrieveAllUsers();

            return Ok(storageUser);
        });

        [HttpGet("{userId}")]
        public ValueTask<ActionResult<User>> GetUserAsync(Guid userId) =>
        TryCatchUserFunction(async () =>
        {
            User storageUser =
                   await this.userService.RetrieveUserByIdAsync(userId);

            return Ok(storageUser);
        });

        [HttpPut]
        public ValueTask<ActionResult<User>> PutUserAsync(User user) =>
        TryCatchUserFunction(async () =>
        {
            User registeredUser =
                    await this.userService.ModifyUserAsync(user);

            return Ok(registeredUser);
        });


        [HttpDelete("{userId}")]
        public ValueTask<ActionResult<User>> DeleteUserAsync(Guid userId) =>
        TryCatchUserFunction(async () =>
        {
            User storageUser =
                    await this.userService.RemoveUserByIdAsync(userId);

            return Ok(storageUser);
        });

    }
}
