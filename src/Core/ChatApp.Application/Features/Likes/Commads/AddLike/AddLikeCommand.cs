using ChatApp.Application.Persistence.Contracts;
using ChatApp.Application.Responses;
using ChatApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Features.Likes.Commads.AddLike;
public class AddLikeCommand : IRequest<BaseCommonResponse>
{
    public string UserName { get; set; }

    public AddLikeCommand(string userName)
    {
        UserName = userName;
    }

    class Handler : IRequestHandler<AddLikeCommand, BaseCommonResponse>
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;

        public Handler(ILikeRepository likeRepository, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _likeRepository = likeRepository;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        public async Task<BaseCommonResponse> Handle(AddLikeCommand request, CancellationToken cancellationToken)
        {
            BaseCommonResponse res = new();

            try
            {
                if (!string.IsNullOrEmpty(request.UserName))
                {
                    var currenUser = _httpContextAccessor?.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;

                    var sourceUser = await _userManager.FindByNameAsync(currenUser);

                    var destUser = await _userManager.FindByNameAsync(request.UserName);

                    if (sourceUser is not null && destUser is not null)
                    {
                        if (sourceUser.UserName == request.UserName)
                        {
                            res.IsSuccess = false;
                            res.Message = "Can't like yourself";
                            return res;
                        }
                        var usrLike = await _likeRepository.GetUserLike(sourceUser.Id, destUser.Id);
                        if(usrLike is not null)
                        {
                            res.IsSuccess = false;
                            res.Message = "you alerdy liked this user";
                            return res;
                        }
                    }

                    var result = await _likeRepository.AddLike(destUser.Id, sourceUser.Id);
                    if (result)
                    {
                        res.IsSuccess = true;
                        res.Message = "Add Like Successfully";
                        return res;
                    }
                }
                res.IsSuccess = false;
                res.Message = "username not found";
                return res;
            }
            catch (Exception ex)
            {   res.IsSuccess = false;
                res.Message = ex.Message;

                return res;
            }
        }
    }
}
