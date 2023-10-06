namespace ChatApp.Application.Features.Accounts.Queries.GetAllUsers;

public class PhotoDto
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public bool IsMain { get; set; }

}