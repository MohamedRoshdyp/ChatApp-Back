using AutoMapper;
using ChatApp.Application.Persistence.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Features.Accounts.Queries.GetAllUsers;
public class GetAllUsersQuery:IRequest<List<MemberDto>>
{
    class Handler : IRequestHandler<GetAllUsersQuery, List<MemberDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public Handler(IUserRepository userRepository,IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<List<MemberDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var users = await _userRepository.GetUsersAsync();
                var res = _mapper.Map<List<MemberDto>>(users);
                return res;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
