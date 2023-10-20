using ChatApp.Application.Features.Admin.Commands.UpdateRoles;
using ChatApp.Application.Features.Admin.Queries.GetUsersWithRoles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers;

public class AdminController : BaseController
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator) : base(mediator)
    {
        _mediator = mediator;
    }
    /// <summary>
    /// Only Admin Can Modify Or View All Users With Roles and Edit them
    /// </summary>
    /// <returns></returns>
    [Authorize(Policy ="RequiredAdminRole")]
    [HttpGet("get-users-with-roles")]
    public async Task<ActionResult<List<UsersWithRolesDto>>> GetUsersWithRoles()
    {
        try
        {
            var query = new GetUsersWithRolesQuery();
            var response = await _mediator.Send(query);
            if(response is not null)
                return Ok(response);

            return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("update-roles/{userName}")]
    public async Task<IActionResult> UpdateRole(string userName, [FromQuery] string roles)
    {
        // Admin,Member,......
        var command = new UpdateRolesCommand(userName, roles);
        var response = await _mediator.Send(command, CancellationToken.None);
        if(response.IsSuccess == false && response.Message == "badRequest")
        {
            return BadRequest(response.Errors);
        }
        if (response.IsSuccess)
        {
            return Ok(response.Data);

        }
        return NotFound("this userName not found");
    }

}
