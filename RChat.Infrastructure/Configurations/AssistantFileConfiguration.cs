using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RChat.Domain.AssistantFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Infrastructure.Configurations
{
    internal class AssistantFileConfiguration : IEntityTypeConfiguration<AssistantFile>
    {
        public void Configure(EntityTypeBuilder<AssistantFile> builder)
        {
            builder.HasOne(af => af.Assistant).WithMany(a => a.AssistantFiles);
        }
    }
}
