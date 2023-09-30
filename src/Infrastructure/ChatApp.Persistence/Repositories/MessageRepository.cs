using ChatApp.Application.Persistence.Contracts;
using ChatApp.Persistence.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Persistence.Repositories;
public class MessageRepository : GenericRepository<Domain.Entities.Message>, IMessageRepository
{
    public MessageRepository(ApplicationDbContext context) : base(context)
    {
    }
    //for future
}
