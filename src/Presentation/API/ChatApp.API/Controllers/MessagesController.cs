using ChatApp.Application.Features.Message.Command.AddMessage;
using ChatApp.Application.Features.Message.Command.DeleteMessage;
using ChatApp.Application.Features.Message.Command.GEtMessageRead;
using ChatApp.Application.Features.Message.Query.GetAllMessages;
using ChatApp.Application.Features.Message.Query.GetMessageForUser;
using ChatApp.Application.Helper;
using ChatApp.Application.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers;

public class MessagesController : BaseController
{
    private readonly IMediator _mediator;

    public MessagesController(IMediator mediator) : base(mediator)
    {
        _mediator = mediator;
    }
    /// <summary>
    /// Add New Message 
    /// </summary>
    /// <param name="addMessageDto"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <remarks>
    /// baseURL+api/Messages/add-message
    /// </remarks>
    [HttpPost("add-message")]
    public async Task<IActionResult> AddMessage([FromBody] AddMessageDto addMessageDto, CancellationToken ct)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var command = new AddMessageCommand(addMessageDto);
                var response = await _mediator.Send(command, ct);
                return response.IsSuccess ? Ok(response.Data) : BadRequest(response.Message);
            }
            return BadRequest("Error While Adding New Message , Modelstate invalid");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    /// <summary>
    /// Get All Message For Current User
    /// </summary>
    /// <param name="messageParams"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <remarks>
    /// baseURL+api/Messages/get-message-for-user
    /// </remarks>
    [HttpGet("get-message-for-user")]
    public async Task<ActionResult<MessageDto>> GetMessageForUser([FromQuery] MessageParams messageParams, CancellationToken ct)
    {
        var messages = await _mediator.Send(new GetMessageForUserQuery(messageParams), ct);

        Response.AddPaginationHeader(messages.CurrentPage, messageParams.PageSize, messages.TotalCount, messages.TotalPages);

        if (messages is not null)
        {

            return Ok(messages);
        }
        return NotFound();
    }

    /// <summary>
    /// Get All Message That Read By Using Recipient UserName
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <remarks>
    /// baseURL+api/Messages/get-message-read/ali
    /// </remarks>
    [HttpGet("get-message-read/{userName}")]
    public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageRead(string userName,CancellationToken ct)
    {
        try
        {
            var response = await _mediator.Send(new GetMessageReadCommand(userName),ct);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    /// <summary>
    /// Delete Message When Two point are the same status in Deleted
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <remarks> 
    /// baseURL+api/Messages/delete-message/1
    /// </remarks>
    [HttpDelete("delete-message/{id}")]
    public async Task<IActionResult> DeleteMessage(int id)
    {
        try
        {
            var command = new DeleteMessageCommand(id);
            var response = await _mediator.Send(command, CancellationToken.None);
            if(response.IsSuccess == false && response.Message == "Unauthorized")
            {
                return Unauthorized();
            }
            if(response.IsSuccess&& response.Message == "ok")
            {
                return Ok();
            }
            return NotFound($"this message id `{id}` not found");
        }
        catch (Exception ex)
        {

            return BadRequest(ex.Message);
        }
    }
        
}
