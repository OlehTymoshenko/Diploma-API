using System;
using AutoMapper;
using DL.Entities;
using BL.Models.Auth;

namespace BL.Infrastructure.AutoMapperProfiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<RegisterModel, User>()
                .ForMember(u => u.Id, opt => opt.Ignore())
                .ForMember(u => u.CreateDateTime, opt => opt.MapFrom(_ => DateTime.UtcNow));
        }
    }
}
