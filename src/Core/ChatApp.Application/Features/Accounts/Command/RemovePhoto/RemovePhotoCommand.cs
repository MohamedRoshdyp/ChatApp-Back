using ChatApp.Application.Persistence.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Features.Accounts.Command.RemovePhoto;
public class RemovePhotoCommand:IRequest<bool>
{
    public int Id { get; set; }

    public RemovePhotoCommand( int id)
    {
        Id = id;
    }

    class Handler : IRequestHandler<RemovePhotoCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public Handler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<bool> Handle(RemovePhotoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id>0)
                {
                    //remove
                    await _userRepository.RemovePhoto(request.Id);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;

                throw;
            }
        }
    }
}
