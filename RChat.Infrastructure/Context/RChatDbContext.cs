
using Microsoft.EntityFrameworkCore;
using RChat.Domain.Attachments;
using RChat.Domain.Chats;
using RChat.Domain.Messages;
using RChat.Domain.Users;
using RChat.Infrastructure.Configurations;

namespace RChat.Infrastructure.Context;

public partial class RChatDbContext : DbContext
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

    public virtual DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new AttachmentConfiguration());
        modelBuilder.ApplyConfiguration(new MessageConfiguration());
        modelBuilder.ApplyConfiguration(new ChatConfiguration());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
