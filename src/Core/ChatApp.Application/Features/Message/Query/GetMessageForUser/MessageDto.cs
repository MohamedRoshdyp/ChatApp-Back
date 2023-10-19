using ChatApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Features.Message.Query.GetMessageForUser;
public class MessageDto
{
    public int Id { get; set; }
    public string? SenderId { get; set; }
    public string? SenderUserName { get; set; } 
    public string? SenderProfileUrl { get; set; }

    public string? RecipientId { get; set; }
    public string? RecipientUserName { get; set; }
    public string? RecipientProfileUrl { get; set; }

    public string? Content { get; set; } 

    public DateTime? DateRead { get; set; }
    public DateTime MessageSend { get; set; }


}
