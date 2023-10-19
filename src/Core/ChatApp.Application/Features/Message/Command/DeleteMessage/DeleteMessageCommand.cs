using ChatApp.Application.Persistence.Contracts;
using ChatApp.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Features.Message.Command.DeleteMessage;
public class DeleteMessageCommand:IRequest<BaseCommonResponse>
{
    public int Id { get; set; }

    public DeleteMessageCommand(int id)
    {
        Id = id;
    }

    class Handler : IRequestHandler<DeleteMessageCommand, BaseCommonResponse>
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Handler(IMessageRepository messageRepository,IHttpContextAccessor httpContextAccessor)
        {
            _messageRepository = messageRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<BaseCommonResponse> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
        {
            BaseCommonResponse res = new();
            try
            {
                var userName = _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;

                var message = await _messageRepository.GetMessage(request.Id);
                if(message.Sender.UserName != userName && message.Recipient.UserName != userName)
                {
                    res.IsSuccess = false;
                    res.Message = "Unauthorized";
                    return res;
                }
                if (message.Sender.UserName == userName) message.SenderDeleted = true;
                if (message.Recipient.UserName == userName) message.RecipientDeleted = true;

                await _messageRepository.UpdateAsync(message);

                if (message.SenderDeleted && message.RecipientDeleted)
                    _messageRepository.DeleteMessage(message);

                res.Message = "ok";
                res.IsSuccess = true;
                return res;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
