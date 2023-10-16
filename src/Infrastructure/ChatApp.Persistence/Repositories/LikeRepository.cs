using ChatApp.Application.Extensions;
using ChatApp.Application.Features.Likes.Commads.AddLike;
using ChatApp.Application.Helper;
using ChatApp.Application.Helpers;
using ChatApp.Application.Persistence.Contracts;
using ChatApp.Domain.Entities;
using ChatApp.Persistence.DatabaseContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Persistence.Repositories;
public class LikeRepository : ILikeRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<AppUser> _userManager;
    private readonly IConfiguration _config;

    public LikeRepository(ApplicationDbContext context,IHttpContextAccessor httpContextAccessor,UserManager<AppUser> userManager,IConfiguration config)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _config = config;
    }

    public async Task<bool> AddLike(string LikedUserId,string SourceUserId)
    {      
            UserLike userLike = new()
            {
                LikedUserId = LikedUserId,
                SourceUserId = SourceUserId
            };
            await _context.UserLikes.AddAsync(userLike);
            await _context.SaveChangesAsync();    
        return true;
    }

    public async Task<UserLike> GetUserLike(string sourceUserId, string likedUserId)
        => await _context.UserLikes.FindAsync(sourceUserId, likedUserId);

    public async Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams)
    {
        var users = _context.Users.Include(x=>x.Photos).OrderBy(x => x.UserName).AsQueryable();
        var likes = _context.UserLikes.AsQueryable();

        if (likesParams.Pridicate == "liked")
        {
            likes = likes.Where(x => x.SourceUserId == likesParams.UserId);
            users = likes.Select(x => x.LikedUser);
        }
        if (likesParams.Pridicate == "likedBy")
        {
            likes = likes.Where(x => x.LikedUserId == likesParams.UserId);
            users = likes.Select(x => x.SourceUser);
        }
        var likedUser =  users.Select(x=>new LikeDto
        {
            Id = x.Id,
            Age = x.DateOfBirth.CalculateAge(),
            City = x.City,
            KnownAs = x.KnownAs,
            UserName = x.UserName,
            PhotoUrl = _config["ApiURL"] +x.Photos.FirstOrDefault(x=>x.IsMain && x.IsActive).Url
        });

        return await PagedList<LikeDto>.CreateAsync(likedUser, likesParams.PageNumber, likesParams.PageSize);
    }

    public async Task<AppUser> GetUserWithLike(string userId)
    {
        return await _context.Users
            .Include(x => x.LikeUser)
            .FirstOrDefaultAsync(x => x.Id == userId);
    }
}
