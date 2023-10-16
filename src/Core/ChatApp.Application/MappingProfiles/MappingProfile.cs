using AutoMapper;
using ChatApp.Application.Extensions;
using ChatApp.Application.Features.Accounts.Command.Register;
using ChatApp.Application.Features.Accounts.Queries.GetAllUsers;
using ChatApp.Application.Features.Message.Command.AddMessage;
using ChatApp.Application.Features.Message.Query.GetAllMessages;
using ChatApp.Application.Helper;
using ChatApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.MappingProfiles;
public class MappingProfile:Profile
{
    private const string baserURL = "https://localhost:44318/";
    public MappingProfile()
    {
        //mapping Message
        CreateMap<Message, AddMessageDto>().ReverseMap();
        CreateMap<Message, MessageReturnDto>().ReverseMap();

        //mapping appuser-memeberdto
        CreateMap<AppUser, MemberDto>()           
             .ForMember(d => d.PhotoUrl, o => o.MapFrom( s => baserURL+ s.Photos.FirstOrDefault(x=>x.IsMain).Url))
             //.ForMember(d => d.PhotoUrl, o => o.MapFrom<PhotoMemberResolver>())
             .ForMember(d=>d.Age,o=>o.MapFrom(s=>s.DateOfBirth.CalculateAge()))
             .ReverseMap();
        CreateMap<Photo, PhotoDto>()
            .ForMember(d=>d.Url,o=>o.MapFrom<UserPhotoResolver>())
            .ReverseMap();

        //mapping appuser-registerDto
        CreateMap<AppUser, RegisterDto>().ReverseMap();
    }
}
