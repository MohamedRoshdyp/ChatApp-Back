using ChatApp.Application.Responses;
using ChatApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Features.Admin.Commands.UpdateRoles;
public class UpdateRolesCommand:IRequest<BaseCommonResponse>
{
    public string UserName { get; set; }
    public string Roles { get; set; }

    public UpdateRolesCommand(string userName, string roles)
    {
        UserName = userName;
        Roles = roles;
    }

    class Handler : IRequestHandler<UpdateRolesCommand, BaseCommonResponse>
    {
        private readonly UserManager<AppUser> _userManager;

        public Handler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<BaseCommonResponse> Handle(UpdateRolesCommand request, CancellationToken cancellationToken)
        {
            BaseCommonResponse response = new();
            try
            {
                //Admin,Member,....
                var selectedRoles = request.Roles.Split(",").ToArray();
                var user = await _userManager.FindByNameAsync(request.UserName);
                if(user is not null)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
                    if (!result.Succeeded)
                    {
                        response.IsSuccess = false;
                        response.Message = "badRequest";
                        response.Errors.Add("failed adding this role");
                        return response;
                    }
                    result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
                    if (!result.Succeeded)
                    {
                        response.IsSuccess = false;
                        response.Message = "badRequest";
                        response.Errors.Add("failed removing this role");
                        return response;
                    }
                 

                }
                response.IsSuccess = true;
                response.Message = "ok";
                response.Data = await _userManager.GetRolesAsync(user);
                return response;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "badRequest";
                response.Errors.Add(ex.Message);
                return response;
            }
        }
    }

}
