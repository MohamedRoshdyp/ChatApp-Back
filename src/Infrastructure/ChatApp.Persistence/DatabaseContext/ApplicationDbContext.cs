using ChatApp.Domain.Common;
using ChatApp.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Persistence.DatabaseContext;
public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<UserLike>()
            .HasKey(k => new { k.SourceUserId, k.LikedUserId });
        modelBuilder.Entity<UserLike>()
            .HasOne(x => x.SourceUser)
            .WithMany(x => x.LikeUser)
            .HasForeignKey(x => x.SourceUserId);

        modelBuilder.Entity<UserLike>()
            .HasOne(x => x.LikedUser)
            .WithMany(x => x.LikedByUser)
            .HasForeignKey(x => x.LikedUserId);

    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Modified)
                entry.Entity.ModifiedDate = DateTime.UtcNow;

        }
        return base.SaveChangesAsync(cancellationToken);
    }
    public DbSet<ChatApp.Domain.Entities.Message> Messages { get; set; }
    public DbSet<ChatApp.Domain.Entities.Photo> Photos { get; set; }
    public DbSet<ChatApp.Domain.Entities.UserLike> UserLikes { get; set; }

}
