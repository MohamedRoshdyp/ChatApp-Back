using AutoMapper;
using ChatApp.Application.Features.Accounts.Queries.GetAllUsers;
using ChatApp.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Helper;
public class PhotoMemberResolver:IValueResolver<AppUser,MemberDto, string>
{
    private readonly IConfiguration _config;

    public PhotoMemberResolver(IConfiguration config)
    {
        _config = config;
    }

    public string Resolve(AppUser source, MemberDto destination, string destMember, ResolutionContext context)
    {
        return _config["ApiURL"] + source?.Photos?.FirstOrDefault(x=>x.IsMain && x.IsActive)?.Url;
    }
}
