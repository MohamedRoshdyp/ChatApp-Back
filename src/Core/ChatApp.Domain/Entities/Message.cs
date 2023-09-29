using ChatApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Domain.Entities;
public class Message : BaseEntity
{
    public int SenderId { get; set; }
    public string SenderUserName { get; set; } = string.Empty;

    public int RecipientId { get; set; }
    public string RecipientUserName { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTime? DateRead { get; set; }
    public DateTime MessageSend { get; set; } = DateTime.UtcNow;


    public bool SenderDeleted { get; set; }
    public bool RecipientDeleted { get; set; }
}
