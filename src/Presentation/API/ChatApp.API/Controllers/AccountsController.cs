using ChatApp.Application.Features.Accounts.Command.CheckUserNameOrEmailExist;
using ChatApp.Application.Features.Accounts.Command.Login;
using ChatApp.Application.Features.Accounts.Command.Register;
using ChatApp.Application.Features.Accounts.Command.RemovePhoto;
using ChatApp.Application.Features.Accounts.Command.SetMainPhoto;
using ChatApp.Application.Features.Accounts.Command.UpdateCurrentMember;
using ChatApp.Application.Features.Accounts.Command.UploadPhoto;
using ChatApp.Application.Features.Accounts.Queries.GetAllUsers;
using ChatApp.Application.Features.Accounts.Queries.GetCurrentUser;
using ChatApp.Application.Features.Accounts.Queries.GetUserByUserId;
using ChatApp.Application.Features.Accounts.Queries.GetUserByUserName;
using ChatApp.Application.Helpers;
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
    [AllowAnonymous]
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
    [AllowAnonymous]

    public async Task<ActionResult<RegisterDto>> Register([FromBody] RegisterDto registerDto)
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
    public async Task<ActionResult<MemberDto>> GetAllUsers([FromQuery]UserParams userParams,CancellationToken ct)
    {
        try
        {
            var users = await _mediator.Send(new GetAllUsersQuery(userParams), ct);

            if(users is not null)
            {
                Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
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

    [HttpPut("update-current-member")]
    public async Task<ActionResult<UpdateCurrentMemberDto>> UpdateCurrentMember([FromBody] UpdateCurrentMemberDto updateCurrentMemberDto)
    {
        try
        {
            var command = new UpdateCurrentMemberCommand(updateCurrentMemberDto);
            var response =await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response.Data);
            }
            return BadRequest(response.Errors);
        }
        catch (Exception)
        {

            throw;
        }
    }

    /// <summary>
    /// This Function Take File(Image) and Added To Photo Table 
    /// api/Accounts/upload-photo
    /// </summary>
    /// <param name="file"></param>
    /// <returns>
    /// Object Of Photo Class
    /// </returns>
    /// <remarks>
    /// Take From File 
    /// </remarks>
    [HttpPost("upload-photo")]
    public async Task<ActionResult<PhotoDto>> UploadPhoto(IFormFile file)
    {
        try
        {
            var command = new UploadPhotoCommand{ PhotoFile = file};
            var response = await _mediator.Send(command);
            if (response is not null)
                return Ok(response);
            return BadRequest("Unable to upload photo");
        }
        catch (Exception ex)
        {
            return BadRequest($"Unable to upload photo {ex.Message}");

        }
    }
    /// <summary>
    /// This Endpoint Remove Member Photo 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <remarks>
    /// api/Accounts/remove-photo/5
    /// </remarks>
    [HttpDelete("remove-photo/{id}")]
    public async Task<IActionResult> RemovePhoto(int id)
    {
        try
        {
            var command = new RemovePhotoCommand(id);
            var response = await _mediator.Send(command);
            if (response)
                return Ok("Remove Photo Successfully");
            return BadRequest("Unable to Remove photo");
        }
        catch (Exception ex)
        {
            return BadRequest($"Unable to Remove photo {ex.Message}");

        }
    }

    /// <summary>
    /// This End point Responsible for Set Main Photo
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <remarks>
    /// URL:=> api/Accounts/set-main-photo/2
    /// </remarks>
    [HttpPut("set-main-photo/{id}")]
    public async Task<IActionResult> SetMainPhoto(int id)
    {
        try
        {
            if (id > 0)
            {
                var command = new SetMainPhotoCommand(id);
                var response = await _mediator.Send(command);
                if (response)
                    return Ok("assign successfully");

            }
            return NotFound($"This id {id} doesn't found");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
