using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<AppUser> Users { get; set; }
        public DbSet<UserLike> Likes { get; set; }

        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Configure the UserLike entity to prevent duplicate likes
            builder.Entity<UserLike>()
                .HasKey(l => new { l.SourceUserId, l.TargetUserId });

            builder.Entity<UserLike>()
                .HasOne(l => l.SourceUser)
                .WithMany(u => u.LikedUsers)
                .HasForeignKey(l => l.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserLike>()
                .HasOne(l => l.TargetUser)
                .WithMany(l => l.LikedByUsers)
                .HasForeignKey(l => l.TargetUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Message>()
                .HasOne(r => r.Recipient)
                .WithMany(u => u.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                    .HasOne(r => r.Sender)
                    .WithMany(u => u.MessagesSend)
                    .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
