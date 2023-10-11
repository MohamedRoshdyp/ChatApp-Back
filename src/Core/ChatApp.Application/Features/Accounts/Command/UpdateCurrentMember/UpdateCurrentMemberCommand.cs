using AutoMapper;
using ChatApp.Application.Responses;
using ChatApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Features.Accounts.Command.UpdateCurrentMember;
public class UpdateCurrentMemberCommand:IRequest<BaseCommonResponse>
{
    public UpdateCurrentMemberDto UpdateCurrentMemberDto { get; set; }

    public UpdateCurrentMemberCommand(UpdateCurrentMemberDto updateCurrentMemberDto)
    {
        UpdateCurrentMemberDto = updateCurrentMemberDto;
    }
    class Handler : IRequestHandler<UpdateCurrentMemberCommand, BaseCommonResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;

        public Handler(UserManager<AppUser> userManager,IMapper mapper,IHttpContextAccessor httpContext)
        {
            _userManager = userManager;
            _mapper = mapper;
            _httpContext = httpContext;
        }
        public async Task<BaseCommonResponse> Handle(UpdateCurrentMemberCommand request, CancellationToken cancellationToken)
        {
            BaseCommonResponse response = new();
            try
            {
                var userName = _httpContext?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
                if(userName is not null)
                {
                    
                    var currentUser = await _userManager.FindByNameAsync(userName);
                    currentUser.Introduction = request.UpdateCurrentMemberDto.Introduction;
                    currentUser.Interests = request.UpdateCurrentMemberDto.Interests;
                    currentUser.LookingFor = request.UpdateCurrentMemberDto.LookingFor;
                    currentUser.City = request.UpdateCurrentMemberDto.City;
                    currentUser.Country = request.UpdateCurrentMemberDto.Country;
                    var res = await _userManager.UpdateAsync(currentUser);
                    if (res.Succeeded)
                    {
                        response.IsSuccess = true;
                        response.Message = "Update Current User Successfuly!";
                        response.Data = request.UpdateCurrentMemberDto;
                        return response;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        foreach (var  err in res.Errors)
                        {
                            response.Errors.Add($"code {err.Code} - description {err.Description}");
                        }
                        response.Message = "badRequest";
                        return response;
                    }
                }
                response.IsSuccess = false;
                response.Message = "User Name Incorrect!";
                return response;
            }
            catch (Exception ex)
            {

                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
        }
    }

}
