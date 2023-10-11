using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Features.Accounts.Command.UpdateCurrentMember;
public class UpdateCurrentMemberDto
{
    public string? Introduction { get; set; }
    public string? LookingFor { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Interests { get; set; }
}
