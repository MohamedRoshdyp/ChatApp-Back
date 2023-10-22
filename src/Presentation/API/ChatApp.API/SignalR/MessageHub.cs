using AutoMapper;
using Azure.Core;
using ChatApp.Application.Features.Message.Command.AddMessage;
using ChatApp.Application.Persistence.Contracts;
using ChatApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace ChatApp.API.SignalR;

public class MessageHub : Hub
{
    private readonly IMessageRepository _messageRepository;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;

    public MessageHub(IMessageRepository messageRepository, IMediator mediator, IMapper mapper, UserManager<AppUser> userManager)
    {
        _messageRepository = messageRepository;
        _mediator = mediator;
        _mapper = mapper;
        _userManager = userManager;
    }
    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var otherUser = httpContext?.Request?.Query["user"].ToString();
        var currentUserName = Context?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
        var groupName = GetGroupName(currentUserName, otherUser);
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        var message = await _messageRepository.GetMessageRead(currentUserName, otherUser);
        await Clients.Group(groupName).SendAsync("ReceivedMessageRead", message);
    }

    private string GetGroupName(string caller, string other)
    {
        var stringCompare = string.CompareOrdinal(caller, other) < 0;
        return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
    }

    //send message
    public async Task SendMessage(AddMessageDto addMessageDto)
    {
        var message = _mapper.Map<Domain.Entities.Message>(addMessageDto);

        message.SenderId = Context?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;


        message.SenderUserName = Context?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value ?? "";

        var recipient = await _userManager.Users.Include(x => x.Photos).FirstOrDefaultAsync(x => x.UserName == message.RecipientUserName);

        var sender = await _userManager.Users.Include(x => x.Photos).FirstOrDefaultAsync(x => x.UserName == message.SenderUserName);

        message.RecipientId = recipient?.Id;
        await _messageRepository.AddAsync(message);
        var caller = sender.UserName;
        var other = recipient.UserName;

        var groupName = GetGroupName(caller, other);
        await Clients.Group(groupName).SendAsync("NewMessage", message);
    }



    //public async Task SendMessage(AddMessageDto addMessageDto)
    //{
    //    var command = new AddMessageCommand(addMessageDto);
    //    var response = await _mediator.Send(command);
    //    if (response.IsSuccess == false) throw new HubException(response.Message);

    //    dynamic dataObject = JsonConvert.DeserializeObject(response.Data.ToString());

    //    var caller = dataObject.SenderUserName;
    //    var other = dataObject.RecipientUserName;

    //    var groupName = GetGroupName(caller, other);
    //    await Clients.Group(groupName).SendAsync("NewMessage", response.Data);
    //    //return response.IsSuccess ? Ok(response.Data) : BadRequest(response.Message);
    //}


    public override Task OnDisconnectedAsync(Exception? exception)
    {
        return base.OnDisconnectedAsync(exception);
    }
}
