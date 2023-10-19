using ChatApp.Application.Features.Message.Query.GetMessageForUser;
using ChatApp.Application.Persistence.Contracts;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Features.Message.Command.GEtMessageRead;
public class GetMessageReadCommand:IRequest<IEnumerable<MessageDto>>
{
    public string CurrentUserName { get; set; }
    public string RecipientUserName { get; set; }

    public GetMessageReadCommand(string recipientUserName)
    {
        RecipientUserName = recipientUserName;
    }

    class Handler : IRequestHandler<GetMessageReadCommand, IEnumerable<MessageDto>>
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Handler(IMessageRepository messageRepository,IHttpContextAccessor httpContextAccessor)
        {
            _messageRepository = messageRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IEnumerable<MessageDto>> Handle(GetMessageReadCommand request, CancellationToken cancellationToken)
        {
            try
            {
                request.CurrentUserName = _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;

                var message = await _messageRepository.GetMessageRead(request.CurrentUserName, request.RecipientUserName);

                return message;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
