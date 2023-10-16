using ChatApp.Application.Features.Likes.Commads.AddLike;
using ChatApp.Application.Helper;
using ChatApp.Application.Helpers;
using ChatApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Persistence.Contracts;
public interface ILikeRepository
{
    Task<bool> AddLike(string LikedUserId, string SourceUserId);
    Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams);
    Task<UserLike> GetUserLike(string sourceUserId, string likedUserId);
    Task<AppUser> GetUserWithLike(string userId);
}
