using ChatApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Features.Accounts.Queries.GetAllUsers;
public class MemberDto
{
    public string? Id { get; set; }
    public string? UserName { get; set; }
    public int Age { get; set; }
    public string? PhotoUrl { get; set; }
    public string? KnownAs { get; set; } 
    public DateTime Created { get; set; } 
    public DateTime LastActive { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string Introduction { get; set; } = string.Empty;
    public string LookingFor { get; set; } = string.Empty;
    public string Interests { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public ICollection<PhotoDto> Photos { get; set; }
}
