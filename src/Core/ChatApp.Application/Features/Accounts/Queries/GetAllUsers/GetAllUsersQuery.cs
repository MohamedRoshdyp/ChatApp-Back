using AutoMapper;
using ChatApp.Application.Helpers;
using ChatApp.Application.Persistence.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Features.Accounts.Queries.GetAllUsers;
public class GetAllUsersQuery:IRequest<PagedList<MemberDto>>
{
    public UserParams UserParams { get; set; }

    public GetAllUsersQuery(UserParams userParams)
    {
        UserParams = userParams;
    }

    class Handler : IRequestHandler<GetAllUsersQuery, PagedList<MemberDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public Handler(IUserRepository userRepository,IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<PagedList<MemberDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var users = await _userRepository.GetMembersAsync(request.UserParams);
                
                //var res = _mapper.Map<List<MemberDto>>(users);
                return users;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
