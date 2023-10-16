using AutoMapper;
using AutoMapper.QueryableExtensions;
using ChatApp.Application.Features.Accounts.Queries.GetAllUsers;
using ChatApp.Application.Helpers;
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
    private readonly IMapper _mapper;

    public UserRepository(ApplicationDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment webHost, IHttpContextAccessor httpContext, IMapper mapper)
    {
        _context = context;
        _userManager = userManager;
        _webHost = webHost;
        _httpContext = httpContext;
        _mapper = mapper;
    }

    public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
    {
        //current user
        var currentUser = _httpContext?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;


        if (currentUser is not null)
        {

            var user = await _userManager.FindByNameAsync(currentUser);

            //check gender is null
            if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = user?.Gender == "Male" ? "Female" : "Male"; // default
            }
            //reassing Current username
            userParams.CurrentUserName = user?.UserName;
        }
        //filter min-max age
        var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
        var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

        



        var query = _context.Users.Include(x => x.Photos).AsQueryable();
        query = query.Where(x => x.UserName != userParams.CurrentUserName);
        query = query.Where(x => x.Gender == userParams.Gender);
        query = query.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);

        //sorting by lastActive | created
        query = userParams.OrderBy switch
        {
            "created" => query.OrderByDescending(x => x.Created),
            _ => query.OrderByDescending(x => x.LastActive),
        };

        return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(_mapper.ConfigurationProvider).AsNoTracking(), userParams.PageNumber, userParams.PageSize);


    }

    public async Task<AppUser> GetUserByIdAsync(string userId)
    => await _userManager?.Users.Include(x => x.Photos).FirstOrDefaultAsync(x => x.Id == userId);

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

    public async Task UpdateUser(AppUser user)
    {
        await _userManager.UpdateAsync(user);
        _context.SaveChanges();
    }

    public async Task<PhotoDto> UploadPhoto(IFormFile file, string pathName)
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
                    PhotoDto photoDto = new();

                    photo.Url = PhotoManager._uploadPhoto(_webHost, file, pathName);
                    photo.AppUserId = user.Id;
                    photo.IsActive = true;
                    await _context.Photos.AddAsync(photo);
                    await _context.SaveChangesAsync();

                    return _mapper.Map<PhotoDto>(photo);
                }
            }

            //Result = PhotoManager._uploadPhoto(_webHost,file, pathName);
        }
        return null;

    }
}
