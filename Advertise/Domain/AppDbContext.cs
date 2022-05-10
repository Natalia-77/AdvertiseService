using Domain.Entities;
using Domain.ModelBuilderConfig;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :
           base(options)
        {

        }

        public virtual DbSet<Advertise> Advertises { get; set; }
        public virtual DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);
            modelbuilder.ApplyConfiguration(new AdvertiseConfig());
            modelbuilder.ApplyConfiguration(new ImagesConfig());

        }
    }
}
