using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ZionCodes.Core.Dtos.Users;
using ZionCodes.Core.Models.Users;

namespace ZionCodes.Core.Configs
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<RegisterUserDTO, User>()
                .ForMember(user => user.Email, opt =>
                    opt.MapFrom(userDto => userDto.Email))
                .ForMember(user => user.UserName, opt =>
                    opt.MapFrom(userDto => userDto.Email));
        }
    }
}
