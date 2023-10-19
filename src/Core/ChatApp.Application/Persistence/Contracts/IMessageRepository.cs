using ChatApp.Application.Features.Message.Query.GetMessageForUser;
using ChatApp.Application.Helper;
using ChatApp.Application.Helpers;
using ChatApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Persistence.Contracts;
public interface IMessageRepository:IGenericRepository<Message>
{
    //for Future

    void DeleteMessage(Message message);
    Task<Message> GetMessage(int id);
    Task<PagedList<MessageDto>> GetMessageForUser(MessageParams messageParams);
    Task<IEnumerable<MessageDto>> GetMessageRead(string currentUserName, string recipientUserName);
}
