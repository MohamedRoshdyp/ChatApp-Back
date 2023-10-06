using AutoMapper;
using ChatApp.Application.Features.Accounts.Queries.GetAllUsers;
using ChatApp.Application.Persistence.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Features.Accounts.Queries.GetUserByUserName;
public class GetUserByUserNameQuery:IRequest<MemberDto>
{
    public string UserName { get; set; }

    public GetUserByUserNameQuery(string userName)
    {
        UserName = userName;
    }

    class Handler : IRequestHandler<GetUserByUserNameQuery, MemberDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public Handler(IUserRepository userRepository,IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<MemberDto> Handle(GetUserByUserNameQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (!string.IsNullOrEmpty(request.UserName))
                {
                    var user = await _userRepository.GetUserByUserNameAsync(request.UserName);
                    if(user is not null)
                    {
                        return _mapper.Map<MemberDto>(user);
                    }
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
