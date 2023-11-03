using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RChat.Domain.Chats;
using RChat.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Infrastructure.Configurations
{
    internal class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {

            builder.HasKey(e => e.Id).HasName("PK__Chats__3214EC073B91F95D");

            builder.HasOne(d => d.Creator).WithMany(p => p.CreatedChats)
                    .HasForeignKey(d => d.CreatorId)
                    .HasConstraintName("FK__Chats__CreatorId__398D8EEE");

            builder.HasMany(d => d.Users).WithMany(p => p.Chats)
                    .UsingEntity<Dictionary<string, object>>(
                        "ChatUser",
                        r => r.HasOne<User>().WithMany()
                            .HasForeignKey("UsersId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK__ChatUser__UsersI__412EB0B6"),
                        l => l.HasOne<Chat>().WithMany()
                            .HasForeignKey("ChatsId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK__ChatUser__ChatsI__403A8C7D"),
                        j =>
                        {
                            j.HasKey("ChatsId", "UsersId");
                            j.ToTable("ChatUser");
                        });

        }
    }
}
