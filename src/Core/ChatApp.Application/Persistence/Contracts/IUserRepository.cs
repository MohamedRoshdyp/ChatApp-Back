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
    void UpdateUser(AppUser user);

    Task<IEnumerable<AppUser>> GetUsersAsync();

    Task<AppUser> GetUserByIdAsync(string userId);
    Task<AppUser> GetUserByUserNameAsync(string userName);

    Task<bool> UploadPhoto(IFormFile file, string pathName);
    Task<bool> RemovePhoto(int id);
    Task<bool> SetMainPhoto(int id);

}
