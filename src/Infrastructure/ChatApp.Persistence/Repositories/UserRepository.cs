using ChatApp.Application.Persistence.Contracts;
using ChatApp.Domain.Entities;
using ChatApp.Persistence.DatabaseContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Persistence.Repositories;
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public UserRepository(ApplicationDbContext context,UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    public async Task<AppUser> GetUserByIdAsync(string userId)
    => await _userManager.Users.Include(x => x.Photos).FirstOrDefaultAsync(x => x.Id == userId);

    public async Task<AppUser> GetUserByUserNameAsync(string userName)
   => await _userManager.Users.Include(x=>x.Photos).FirstOrDefaultAsync(x=>x.UserName == userName);

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    => await _userManager.Users.Include(x=>x.Photos).ToListAsync();

    public void UpdateUser(AppUser user)
    {
        _userManager.UpdateAsync(user);
        _context.SaveChanges();
    }
}
