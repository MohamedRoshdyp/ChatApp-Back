using ChatApp.Application.Features.Accounts.Queries.GetAllUsers;
using ChatApp.Application.Helpers;
using ChatApp.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Persistence.Contracts;
public interface IUserRepository
{
    Task UpdateUser(AppUser user);

    Task<IEnumerable<AppUser>> GetUsersAsync();

    Task<AppUser> GetUserByIdAsync(string userId);
    Task<AppUser> GetUserByUserNameAsync(string userName);
    Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
    Task<PhotoDto> UploadPhoto(IFormFile file, string pathName);
    Task<bool> RemovePhoto(int id);
    Task<bool> SetMainPhoto(int id);

}
