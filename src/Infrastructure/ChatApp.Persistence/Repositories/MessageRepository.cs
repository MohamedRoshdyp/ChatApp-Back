using AutoMapper;
using AutoMapper.QueryableExtensions;
using ChatApp.Application.Features.Message.Query.GetMessageForUser;
using ChatApp.Application.Helper;
using ChatApp.Application.Helpers;
using ChatApp.Application.Persistence.Contracts;
using ChatApp.Domain.Entities;
using ChatApp.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Persistence.Repositories;
public class MessageRepository : GenericRepository<Domain.Entities.Message>, IMessageRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public MessageRepository(ApplicationDbContext context,IMapper mapper) : base(context)
    {
        _context = context;
        _mapper = mapper;
    }

    public void DeleteMessage(Message message)
    {
        _context.Messages.Remove(message);
        _context.SaveChanges();
    }

    public async Task<Message> GetMessage(int id)
        =>await _context.Messages.Include(x => x.Sender).Include(x => x.Recipient)
            .FirstOrDefaultAsync(x => x.Id == id);


    public async Task<PagedList<MessageDto>> GetMessageForUser(MessageParams messageParams)
    {
        var query = _context.Messages.Include(x=>x.Sender).Include(x=>x.Recipient)
            .OrderByDescending(x => x.MessageSend)
            .AsQueryable();

        query = messageParams.Container switch
        {
            "Inbox" => query.Where(x => x.Recipient.UserName == messageParams.UserName 
            && x.RecipientDeleted == false
            ),
            "Outbox" => query.Where(x => x.Sender.UserName == messageParams.UserName
            && x.SenderDeleted == false
            ),
            _ => query.Where(x => x.Recipient.UserName == messageParams.UserName && x.RecipientDeleted == false && x.DateRead == null),
        };
        var message = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);

        return await PagedList<MessageDto>.CreateAsync(message, messageParams.PageNumber, messageParams.PageSize);
    }

    public async Task<IEnumerable<MessageDto>> GetMessageRead(string currentUserName, string recipientUserName)
    {
        var message = _context.Messages
            .Include(x => x.Sender).ThenInclude(x => x.Photos)
            .Include(x => x.Recipient).ThenInclude(x => x.Photos)
            .Where(x => x.Recipient.UserName == currentUserName && x.RecipientDeleted ==false
            && x.Sender.UserName == recipientUserName || x.Recipient.UserName == recipientUserName
            && x.Sender.UserName == currentUserName && x.SenderDeleted == false
            ).OrderByDescending(x => x.MessageSend)
            .ToList();

        var unReadMessage = message.Where(x => x.DateRead == null && x.Recipient.UserName == currentUserName).ToList();

        if (unReadMessage.Any())
        {
            foreach (var item in unReadMessage)
            {
                item.DateRead = DateTime.Now;
                _context.Messages.Update(item);
                _context.SaveChanges();
            }
        }
        return _mapper.Map<IEnumerable<MessageDto>>(message);
    }
}
