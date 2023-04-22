using Microsoft.EntityFrameworkCore;
using WheresNannyApi.Domain.Entities;

namespace WheresNannyApi.Data.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(x => x.Username)
                    .HasColumnType("varchar(50)")
                    .IsRequired();

                entity.Property(x => x.Password)
                    .HasColumnType("varchar(50)")
                    .IsRequired();

                entity.Property(x => x.Token)
                    .HasColumnType("varchar(max)")
                    .IsRequired(false)
                    ;

                entity.Property(x => x.CreatedIn)
                    .IsRequired(false)
                    .HasColumnType("datetime2");

                entity.Property(x => x.ExpiresIn)
                    .IsRequired(false)
                    .HasColumnType("datetime2");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
