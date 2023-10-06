using AutoMapper;
using ChatApp.Application.Extensions;
using ChatApp.Application.Features.Accounts.Queries.GetAllUsers;
using ChatApp.Application.Features.Message.Command.AddMessage;
using ChatApp.Application.Features.Message.Query.GetAllMessages;
using ChatApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.MappingProfiles;
public class MappingProfile:Profile
{
    public MappingProfile()
    {
        //mapping Message
        CreateMap<Message, AddMessageDto>().ReverseMap();
        CreateMap<Message, MessageReturnDto>().ReverseMap();

        //mapping appuser-memeberdto
        CreateMap<AppUser, MemberDto>()
             .ForMember(d=>d.PhotoUrl,o=>o.MapFrom(s=>s.Photos.FirstOrDefault(x=>x.IsMain).Url))
             .ForMember(d=>d.Age,o=>o.MapFrom(s=>s.DateOfBirth.CalculateAge()))
             .ReverseMap();
        CreateMap<Photo, PhotoDto>().ReverseMap();
    }
}
