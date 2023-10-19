using ChatApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Persistence.Configuration.Entities;
public class MessageSeed : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasData(
            new Message()
            {
                Id=1,
                Content = "test-one",
                SenderId = "af1d2f98-8eaf-4356-b8e8-9cc6abd2dce5",
                SenderUserName = "ali",
                RecipientId = "96a32430-75ff-4ea1-a7c1-963fce22139f",
                RecipientUserName = "ahmed",
                IsActive = true
            }
               //,new Message()
               //{
               //    Id = 2,
               //    Content = "test-two",
               //    SenderId = 1,
               //    SenderUserName = "mohamed",
               //    RecipientId = 1,
               //    RecipientUserName = "ahmed",
               //    IsActive = true
               //},
               //   new Message()
               //   {
               //       Id = 3,
               //       Content = "test-two",
               //       SenderId = 1,
               //       SenderUserName = "basem",
               //       RecipientId = 1,
               //       RecipientUserName = "ahmed",
               //       IsActive = true
               //   }
                  );
    }
}
