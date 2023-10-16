using ChatApp.Application.Features.Likes.Commads.AddLike;
using ChatApp.Application.Features.Likes.Query.GetUserLike;
using ChatApp.Application.Helper;
using ChatApp.Application.Helpers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers;

public class LikesController : BaseController
{
    private readonly IMediator _mediator;

    public LikesController(IMediator mediator) : base(mediator)
    {
        _mediator = mediator;
    }
    /// <summary>
    /// Add Like To List 
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    /// <remarks>baseURL/api/Likes/add-like/{userName}</remarks>
    [HttpPost("add-like/{userName}")]
    public async Task<IActionResult> AddLike(string userName)
    {
        try
        {
            if (!string.IsNullOrEmpty(userName))
            {
                var command = new AddLikeCommand(userName);
                var response =await _mediator.Send(command);
                if(response.IsSuccess)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response.Message);
                }
            }
            return NotFound(value: "username not found");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get User Likes From List 
    /// </summary>
    /// <param name="likesParams"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <remarks>
    /// baseURL/api/Likes/get-user-like
    /// </remarks>
    [HttpGet("get-user-like")]
    public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLike([FromQuery] LikesParams likesParams,CancellationToken ct)
    {
        try
        {
            var likes = await _mediator.Send(new GetUserLikeQuery(likesParams), ct);
            if (likes is not null)
            {
                Response.AddPaginationHeader(likes.CurrentPage,likes.PageSize,likes.TotalCount,likes.TotalPages);
                return Ok(likes);
            }
            return NotFound();
        }
        catch (Exception ex)
        {

            return BadRequest(ex.Message);
        }
    }
    
}
