using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Features.Accounts.Command.Register;
public class RegisterDto
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string KnownAs { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; } 
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
}
