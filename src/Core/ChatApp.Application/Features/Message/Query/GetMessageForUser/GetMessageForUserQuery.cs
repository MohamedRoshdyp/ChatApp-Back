using ChatApp.Application.Helper;
using ChatApp.Application.Helpers;
using ChatApp.Application.Persistence.Contracts;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Features.Message.Query.GetMessageForUser;
public class GetMessageForUserQuery:IRequest<PagedList<MessageDto>>
{
    public MessageParams MessageParams { get; set; }

    public GetMessageForUserQuery(MessageParams messageParams)
    {
        MessageParams = messageParams;
    }

    class Handler : IRequestHandler<GetMessageForUserQuery, PagedList<MessageDto>>
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Handler(IMessageRepository messageRepository,IHttpContextAccessor httpContextAccessor)
        {
            _messageRepository = messageRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<PagedList<MessageDto>> Handle(GetMessageForUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                request.MessageParams.UserName = _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;

                var message = await _messageRepository.GetMessageForUser(request.MessageParams);
                return message;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
