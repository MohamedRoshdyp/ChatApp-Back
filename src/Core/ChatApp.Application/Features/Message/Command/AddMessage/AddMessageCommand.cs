using AutoMapper;
using ChatApp.Application.Features.Message.Validator;
using ChatApp.Application.Persistence.Contracts;
using ChatApp.Application.Responses;
using ChatApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Features.Message.Command.AddMessage;
public class AddMessageCommand:IRequest<BaseCommonResponse>
{
    public AddMessageDto AddMessageDto { get; set; }
    public AddMessageCommand(AddMessageDto addMessageDto)
    {
        AddMessageDto = addMessageDto;
    }

    class Handler : IRequestHandler<AddMessageCommand, BaseCommonResponse>
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _config;

        public Handler(IMessageRepository messageRepository,IMapper mapper,IHttpContextAccessor httpContextAccessor,UserManager<AppUser> userManager,IConfiguration config)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _config = config;
        }
        public async Task<BaseCommonResponse> Handle(AddMessageCommand request, CancellationToken cancellationToken)
        {

            try
            {
                BaseCommonResponse response = new();
                MessageValidator validations = new();
                var validatorResult = await validations.ValidateAsync(request.AddMessageDto);
                if (!validatorResult.IsValid)
                {
                    response.IsSuccess = false;
                    response.Message = "While Adding New Message";
                    response.Errors = validatorResult.Errors.Select(x => x.ErrorMessage).ToList();
                }
                var message = _mapper.Map<Domain.Entities.Message>(request.AddMessageDto);

                message.SenderId = _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;


                message.SenderUserName = _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;

                var recipient = await _userManager.Users.Include(x=>x.Photos).FirstOrDefaultAsync(x=>x.UserName== message.RecipientUserName);

                var sender = await _userManager.Users.Include(x => x.Photos).FirstOrDefaultAsync(x => x.UserName == message.SenderUserName);

                message.RecipientId = recipient?.Id;

                if(recipient is null)
                {
                    response.IsSuccess = false;
                    response.Message = "recipient not found";
                    return response;
                }
                
                if(message.SenderUserName == request.AddMessageDto.RecipientUserName)
                {
                    response.IsSuccess = false;
                    response.Message = "u can't send message to yourself";
                    return response;
                }
                
                await _messageRepository.AddAsync(message);
                response.IsSuccess = true;
                response.Message = "Success Adding New Message";
                response.Data = new
                {
                    Id = message.Id,
                    dateRead = message.DateRead,
                    messageSend = message.MessageSend,
                    SenderId = message.SenderId,
                    SenderUserName = message.SenderUserName,
                    SenderProfileUrl = _config["ApiURL"] + sender.Photos.FirstOrDefault(x => x.IsMain && x.IsActive).Url,

                    RecipientId = message.RecipientId,
                    RecipientUserName = message.RecipientUserName,
                    RecipientProfileUrl = _config["ApiURL"] + recipient.Photos.FirstOrDefault(x => x.IsMain && x.IsActive).Url,

                    Content = message.Content
                };


                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
