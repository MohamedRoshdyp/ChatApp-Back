using ChatApp.Application.Persistence.Contracts;
using ChatApp.Domain.Entities;
using ChatApp.Persistence.DatabaseContext;
using ChatApp.Persistence.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Persistence.Repositories;
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly IWebHostEnvironment _webHost;
    private readonly IHttpContextAccessor _httpContext;

    public UserRepository(ApplicationDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment webHost, IHttpContextAccessor httpContext)
    {
        _context = context;
        _userManager = userManager;
        _webHost = webHost;
        _httpContext = httpContext;
    }
    public async Task<AppUser> GetUserByIdAsync(string userId)
    => await _userManager.Users.Include(x => x.Photos).FirstOrDefaultAsync(x => x.Id == userId);

    public async Task<AppUser> GetUserByUserNameAsync(string userName)
   => await _userManager.Users.Include(x => x.Photos).FirstOrDefaultAsync(x => x.UserName == userName);

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    => await _userManager.Users.Include(x => x.Photos).ToListAsync();

    public async Task<bool> RemovePhoto(int id)
    {
        var currentPhoto = await _context.Photos.FirstOrDefaultAsync(x => x.Id == id);
        if (currentPhoto is not null)
        {
            _context.Photos.Remove(currentPhoto);
            await _context.SaveChangesAsync();
            PhotoManager._removePhoto(_webHost, currentPhoto.Url);
            return true;
        }
        return false;
    }

    public async Task<bool> SetMainPhoto(int id)
    {
        var userName = _httpContext?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
        if (userName is not null)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user is not null)
            {
                var currentPhotos = await _context.Photos.Where(x => x.AppUserId == user.Id).ToListAsync();
                foreach (var item in currentPhotos)
                {
                    item.IsMain = false;
                    _context.Photos.Update(item);
                    _context.SaveChanges();
                }

            }
            var currentPhoto = await _context.Photos.FindAsync(id);
            if (currentPhoto is not null)
            {
                if (currentPhoto.IsMain == false)
                {
                    currentPhoto.IsMain = true;
                    _context.Photos.Update(currentPhoto);
                    _context.SaveChanges();
                    return true;
                }
            }

        }

        return false;
    }

    public void UpdateUser(AppUser user)
    {
        _userManager.UpdateAsync(user);
        _context.SaveChanges();
    }

    public async Task<bool> UploadPhoto(IFormFile file, string pathName)
    {
        if (file is not null)
        {

            var userName = _httpContext?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
            if (userName is not null)
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user is not null)
                {
                    Photo photo = new Photo();

                    photo.Url = PhotoManager._uploadPhoto(_webHost, file, pathName);
                    photo.AppUserId = user.Id;
                    photo.IsActive = true;
                    await _context.Photos.AddAsync(photo);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }

            //Result = PhotoManager._uploadPhoto(_webHost,file, pathName);
        }
        return false;

    }
}
