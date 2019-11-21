using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WhoAreU.Models
{
    public partial class SocialNetworkDBContext : DbContext
    {
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<UserUser> UserUser { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(e => e.Pkpost);

                entity.Property(e => e.Pkpost).HasColumnName("PKPost");

                entity.Property(e => e.Body)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.Fkuser)
                    .IsRequired()
                    .HasColumnName("FKUser")
                    .HasMaxLength(450);

                entity.Property(e => e.PublishDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<UserUser>(entity =>
            {
                entity.HasKey(e => new { e.Ppkfkfollowed, e.Ppkfkfollower });

                entity.Property(e => e.Ppkfkfollowed).HasColumnName("PPKFKFollowed");

                entity.Property(e => e.Ppkfkfollower).HasColumnName("PPKFKFollower");
            });
        }


        public SocialNetworkDBContext(DbContextOptions<SocialNetworkDBContext> options) : base(options)
        { }
    }
}
