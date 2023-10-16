using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Helper;
public class LikesParams:PaginationParams
{
    public string? UserId { get; set; }
    public string? Pridicate { get; set; }
}
