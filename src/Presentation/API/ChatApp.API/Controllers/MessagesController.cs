using ChatApp.Application.Features.Message.Command.AddMessage;
using ChatApp.Application.Features.Message.Query.GetAllMessages;
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
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<MessageReturnDto>> Get(CancellationToken ct)
    {
        var messages = await _mediator.Send(new GetAllMessageQuery(),ct);
        return Ok(messages);
    }
    [HttpPost]
    public async Task<ActionResult<AddMessageDto>> Post([FromBody] AddMessageDto addMessageDto,CancellationToken ct)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var command = new AddMessageCommand(addMessageDto);
                var response =await _mediator.Send(command);
                return response.IsSuccess ? Ok(response) : BadRequest(response.Message); 
            }
            return BadRequest("Error While Adding New Message , Modelstate invalid"); 
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
