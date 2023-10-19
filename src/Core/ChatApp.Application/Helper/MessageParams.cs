using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Helper;
public class MessageParams:PaginationParams
{
    public string? UserName { get; set; }
    public string Container { get; set; } = "unRead";
}
