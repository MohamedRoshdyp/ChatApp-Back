using ChatApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Persistence.Configuration.Entities;
public class UserPhotoSeed : IEntityTypeConfiguration<Photo>
{

    public void Configure(EntityTypeBuilder<Photo> builder)
    {
        builder.HasData(
              new Photo() {Id=1, AppUserId = "af1d2f98-8eaf-4356-b8e8-9cc6abd2dce5", IsActive = true, IsMain = true, Url = "https://xsgames.co/randomusers/assets/avatars/male/1.jpg" },
            new Photo() {Id=2, AppUserId = "af1d2f98-8eaf-4356-b8e8-9cc6abd2dce5", IsActive = true, IsMain = true, Url = "https://xsgames.co/randomusers/assets/avatars/male/50.jpg" },
            new Photo() {Id=3, AppUserId = "af1d2f98-8eaf-4356-b8e8-9cc6abd2dce5", IsActive = true, IsMain = false, Url = "https://xsgames.co/randomusers/assets/avatars/male/60.jpg" }



            );

    }
}


