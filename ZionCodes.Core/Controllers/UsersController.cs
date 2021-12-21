using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ZionCodes.Core.Dtos.Generic;
using ZionCodes.Core.Dtos.Users;
using ZionCodes.Core.Models.Users;
using ZionCodes.Core.Services.Users;
using ZionCodes.Core.Utils;

namespace ZionCodes.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController<User>
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        [HttpPost]
        public ValueTask<ActionResult<User>> PostUserAsync(RegisterUserDTO user) =>
       TryCatchUserFunction(async () =>
       {
           User newUser = this.mapper.Map<User>(user);
           newUser.Id = Guid.NewGuid();

           User persistedUser =
                   await this.userService.RegisterUserAsync(newUser, user.Password);

           return Ok(persistedUser);
       });

        [HttpGet]
        public ActionResult<ICollection<User>> GetAllUsers([FromQuery] PaginationQuery paginationQuery) =>
        TryCatchUserFunction(() =>
        {
            ICollection<User> storageUser =
                    this.userService.RetrieveAllUsers();


            if (paginationQuery != null)
            {
                PaginationFilter filter = new()
                {
                    PageNumber = paginationQuery.PageNumber,
                    PageSize = paginationQuery.PageSize,
                };

                if (paginationQuery.PageNumber < 1 || paginationQuery.PageSize < 1)
                {
                    return Ok(new PagedResponse<ICollection<User>>(storageUser));
                }

                var paginationResponse = PaginationBuilder.CreatePaginatedResponse(filter, storageUser);

                return Ok(paginationResponse);
            }

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
