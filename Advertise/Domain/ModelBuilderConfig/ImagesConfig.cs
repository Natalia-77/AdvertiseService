using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ModelBuilderConfig
{
    public class ImagesConfig : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.ToTable("tblImages");
            builder.HasKey(k => k.Id);
            builder.Property(t => t.Name).HasMaxLength(50).IsRequired();
            
        }
    }
}
