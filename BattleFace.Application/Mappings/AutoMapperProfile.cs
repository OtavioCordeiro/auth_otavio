using AutoMapper;
using BattleFace.Application.DTOs;
using BattleFace.Application.Helpers;
using BattleFace.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace BattleFace.Application.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(x => x.Role.Name)))
                .ForMember(dest => dest.Plan, opt => opt.MapFrom(src => src.Plan.Name)).ReverseMap();

            CreateMap<RegisterRequestDto, User>()
            .ForMember(dest => dest.Credential, opt => opt.MapFrom(src =>
                new Credential
                {
                    PasswordHash = PasswordHasher.HashPassword(src.Password)
                }));

        }
    }
}
