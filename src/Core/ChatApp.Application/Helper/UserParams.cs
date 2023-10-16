using ChatApp.Application.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Helpers;
public class UserParams:PaginationParams
{
    
    public string? CurrentUserName { get; set; }
    public string? Gender { get; set; }
    public int MinAge { get; set; } = 15;
    public int MaxAge { get; set; } = 100;
    public string OrderBy { get; set; } = "lastActive";
}
