using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RChat.Domain.Assistants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Infrastructure.Configurations
{
    internal class AssistantConfiguration : IEntityTypeConfiguration<Assistant>
    {
        public void Configure(EntityTypeBuilder<Assistant> builder)
        {
            builder.HasOne(a => a.Chat).WithOne(c => c.Assistant);
            builder.HasMany(a => a.AssistantFiles).WithOne(af => af.Assistant);
        }
    }
}
