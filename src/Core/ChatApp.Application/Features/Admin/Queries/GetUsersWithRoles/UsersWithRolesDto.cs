namespace ChatApp.Application.Features.Admin.Queries.GetUsersWithRoles;

public class UsersWithRolesDto
{
    public string? Id { get; set; }
    public string? UserName { get; set; }
    public IList<string>? Roles { get; set; }
}
