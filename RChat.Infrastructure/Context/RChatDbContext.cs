
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RChat.Domain.Attachments;
using RChat.Domain.Chats;
using RChat.Domain.Messages;
using RChat.Domain.Users;
using RChat.Infrastructure.Configurations;

namespace RChat.Infrastructure.Context;

public partial class RChatDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public RChatDbContext()
    {
    }
    public RChatDbContext(DbContextOptions<RChatDbContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Attachment> Attachments { get; set; }
    public virtual DbSet<Chat> Chats { get; set; }
    public virtual DbSet<Message> Messages { get; set; }
    public virtual DbSet<User> AspNetUsers { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new AttachmentConfiguration());
        modelBuilder.ApplyConfiguration(new MessageConfiguration());
        modelBuilder.ApplyConfiguration(new ChatConfiguration());

        OnModelCreatingPartial(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
