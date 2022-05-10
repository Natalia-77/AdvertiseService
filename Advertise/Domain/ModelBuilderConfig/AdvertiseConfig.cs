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
    public class AdvertiseConfig : IEntityTypeConfiguration<Advertise>
    {
        public void Configure(EntityTypeBuilder<Advertise> builder)
        {
            builder.ToTable("tblAdvertise");
            builder.HasKey(k => k.Id);            
            builder.Property(t => t.Title).HasMaxLength(200).IsRequired();
            builder.Property(d => d.Description).HasMaxLength(1000).IsRequired();
            builder.Property(p => p.Price).IsRequired();
            builder.HasMany(im => im.Images).WithOne(u=>u.Advertise).HasForeignKey(r=>r.AdvertiseId);
        }
    }
}
