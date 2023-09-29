using AutoMapper;
using ChatApp.Application.Persistence.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Features.Message.Query.GetAllMessages;
public class GetAllMessageQuery:IRequest<List<MessageReturnDto>>
{
    class Handler : IRequestHandler<GetAllMessageQuery, List<MessageReturnDto>>
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;

        public Handler(IMessageRepository messageRepository,IMapper mapper)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
        }
        public async Task<List<MessageReturnDto>> Handle(GetAllMessageQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allMessage = await _messageRepository.GetAllAsync();
                var mappingMesg = _mapper.Map<List<MessageReturnDto>>(allMessage);
                return mappingMesg;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
