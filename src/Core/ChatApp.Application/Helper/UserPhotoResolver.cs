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
internal class UserPhotoResolver : IValueResolver<Photo, PhotoDto, string>
{
    private readonly IConfiguration _config;

    public UserPhotoResolver(IConfiguration config)
    {
        _config = config;
    }
    public string Resolve(Photo source, PhotoDto destination, string destMember, ResolutionContext context)
    {
        if (!string.IsNullOrEmpty(source.Url))
        {
            return _config["ApiURL"] + source.Url;
        }
        return null;
    }
}
