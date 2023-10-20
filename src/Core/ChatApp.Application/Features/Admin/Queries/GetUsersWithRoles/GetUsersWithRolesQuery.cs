using ChatApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Features.Admin.Queries.GetUsersWithRoles;
public class GetUsersWithRolesQuery:IRequest<List<UsersWithRolesDto>>
{
    class Handler : IRequestHandler<GetUsersWithRolesQuery, List<UsersWithRolesDto>>
    {
        private readonly UserManager<AppUser> _userManager;

        public Handler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<List<UsersWithRolesDto>> Handle(GetUsersWithRolesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var users =await _userManager.Users.ToListAsync();
                var usersWithRoles = new List<UsersWithRolesDto>();

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var userDto = new UsersWithRolesDto
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Roles = roles.ToList()
                    };
                    usersWithRoles.Add(userDto);
                }
                return usersWithRoles;
                //var users =  _userManager.Users
                //    .Select(x => new UsersWithRolesDto()
                //    {
                        
                //        Id = x.Id,
                //        UserName = x.UserName,
                //        Roles = _userManager.GetRolesAsync(x).Result
                //    }).ToList();
                //return users;

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
