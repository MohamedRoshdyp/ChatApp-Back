﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Helpers;
public class UserParams
{
    private const int MaxPageSize = 50;
    public int PageNumber { get; set; } = 1;

    private int _pageSize = 10;
    public int PageSize {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;    
    }
    public string? CurrentUserName { get; set; }
    public string? Gender { get; set; }
    public int MinAge { get; set; } = 15;
    public int MaxAge { get; set; } = 100;
    public string OrderBy { get; set; } = "lastActive";
}
