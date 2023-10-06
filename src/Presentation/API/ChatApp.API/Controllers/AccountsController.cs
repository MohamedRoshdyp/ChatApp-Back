using ChatApp.Application.Features.Accounts.Command.CheckUserNameOrEmailExist;
using ChatApp.Application.Features.Accounts.Command.Login;
using ChatApp.Application.Features.Accounts.Command.Register;
using ChatApp.Application.Features.Accounts.Queries.GetAllUsers;
using ChatApp.Application.Features.Accounts.Queries.GetCurrentUser;
using ChatApp.Application.Features.Accounts.Queries.GetUserByUserId;
using ChatApp.Application.Features.Accounts.Queries.GetUserByUserName;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatApp.API.Controllers;

public class AccountsController : BaseController
{
    private readonly IMediator _mediator;

    public AccountsController(IMediator mediator) : base(mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var command = new LoginCommand(loginDto);
                var response = await _mediator.Send(command);
                if (response.IsSuccess)
                {
                    return Ok(response.Data);
                }
                if (response.IsSuccess == false && response.Message == "unAuthorized")
                {
                    return Unauthorized();
                }
                if (response.IsSuccess == false && response.Message == "notFound")
                {
                    return NotFound();
                }
            }
            return BadRequest();

        }
        catch (Exception ex)
        {

            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Take Data From Body
    /// </summary>
    /// <param name="registerDto"></param>
    /// <returns>
    /// Return Token-UserName-Email
    /// </returns>
    /// <remarks>
    /// Roles:[1=Admin,2=Memeber]
    /// //BaserURL+/api/register
    /// </remarks>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var command = new RegisterCommand(registerDto);
                var response = await _mediator.Send(command);
                if (response.IsSuccess)
                {
                    return Ok(response.Data);
                }
                if(response.IsSuccess == false)
                {
                    return BadRequest(response.Errors);
                }
                return BadRequest(response.Message);
            }
            return BadRequest();
        }
        catch (Exception)
        {

            throw;
        }
    }

    [Authorize]
    [HttpGet("get-current-user")]
    public async Task<ActionResult<UserReturnDto>> GetCurrentUser(CancellationToken ct)
    {
        try
        {
            var user = await _mediator.Send(new GetCurrentUserQuery(), ct);
            if(user is not null)
            {
                return Ok(user);
            }
            return BadRequest();
        }
        catch (Exception ex)
        {

            return BadRequest(ex.Message);

        }
    }

    [HttpGet("check-userName-or-email-exist/{searchTerm}")]
    public async Task<ActionResult<bool>> CheckUserNameExist(string searchTerm,CancellationToken ct)
    {
        try
        {
            var result = await _mediator.Send(new CheckUserNameOrEmailExistQuery(searchTerm), ct);
            if (result)
                return Ok(true);
            return NotFound(false);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("get-all-users")]
    public async Task<ActionResult<MemberDto>> GetAllUsers(CancellationToken ct)
    {
        try
        {
            var users = await _mediator.Send(new GetAllUsersQuery(), ct);
            if(users is not null)
            {
                return Ok(users);
            }
            return NotFound();
        }
        catch (Exception ex)
        {

            return BadRequest(ex.Message);
        }
    }
    [HttpGet("get-user-by-userName/{userName}")]
    public async Task<ActionResult<MemberDto>> GetUserByUserName(string userName,CancellationToken ct)
    {
        try
        {
            if(!string.IsNullOrEmpty(userName))
            {
                var user = await _mediator.Send(new GetUserByUserNameQuery(userName), ct);
                if (user is not null) return Ok(user);
                
            }
            return NotFound();
        }
        catch (Exception ex)
        {

            return BadRequest(ex.Message);
        }
    }

    [HttpGet("get-user-by-userId/{userId}")]
    public async Task<ActionResult<MemberDto>> GetUserByUserId(string userId, CancellationToken ct)
    {
        try
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var user = await _mediator.Send(new GetUserByUserIdQuery(userId), ct);
                if (user is not null) return Ok(user);

            }
            return NotFound();
        }
        catch (Exception ex)
        {

            return BadRequest(ex.Message);
        }
    }
}
