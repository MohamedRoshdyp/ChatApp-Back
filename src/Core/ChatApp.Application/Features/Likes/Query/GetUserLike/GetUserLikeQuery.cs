using ChatApp.Application.Features.Likes.Commads.AddLike;
using ChatApp.Application.Helper;
using ChatApp.Application.Helpers;
using ChatApp.Application.Persistence.Contracts;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Features.Likes.Query.GetUserLike;
public class GetUserLikeQuery:IRequest<PagedList<LikeDto>>
{
    public LikesParams LikesParams { get; set; }

    public GetUserLikeQuery(LikesParams likesParams)
    {
        LikesParams = likesParams;
    }

    class Handler : IRequestHandler<GetUserLikeQuery, PagedList<LikeDto>>
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Handler(ILikeRepository likeRepository,IHttpContextAccessor httpContextAccessor)
        {
            _likeRepository = likeRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<PagedList<LikeDto>> Handle(GetUserLikeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                request.LikesParams.UserId = _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                var user = await _likeRepository.GetUserLikes(request.LikesParams);
                if(user is not null)
                {
                    return user;
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
