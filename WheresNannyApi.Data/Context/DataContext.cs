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
                    .IsRequired(false);

                entity.Property(x => x.CreatedIn)
                    .IsRequired(false)
                    .HasColumnType("datetime2");

                entity.Property(x => x.ExpiresIn)
                    .IsRequired(false)
                    .HasColumnType("datetime2");

                entity.HasOne(x => x.Person)
                    .WithOne(x => x.User)
                    .HasForeignKey<Person>(x => x.UserId);
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Persons");

                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(x => x.Fullname)
                    .HasColumnType("varchar(150)")
                    .IsRequired();

                entity.Property(x => x.Email)
                    .HasColumnType("varchar(100)")
                    .IsRequired();

                entity.Property(x => x.Cellphone)
                    .HasColumnType("varchar(50)")
                    .IsRequired();

                entity.Property(x => x.BirthdayDate)
                    .HasColumnType("datetime")
                    .IsRequired();

                entity.Property(x => x.Cpf)
                    .HasColumnType("varchar(11)")
                    .IsRequired();

                entity.Property(X => X.ImageUri)
                    .HasColumnType("varchar(max)")
                    .IsRequired();

                entity.HasOne(x => x.Address)
                    .WithOne(y => y.Person)
                    .HasForeignKey<Address>(y => y.PersonId);

                entity.HasOne(x => x.Nanny)
                    .WithOne(y => y.Person)
                    .HasForeignKey<Nanny>(y => y.PersonId);

            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Addresses");

                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(x => x.Cep)
                    .HasColumnType("varchar(8)")
                    .IsRequired();

                entity.Property(x => x.HouseNumber)
                    .HasColumnType("varchar(4)")
                    .IsRequired(false);

                entity.Property(x => x.Complement)
                    .HasColumnType("varchar(50)")
                    .IsRequired(false);
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Services");

                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(x => x.ServiceFinishHour)
                    .HasColumnType("time")
                    .IsRequired();

                entity.Property(x => x.HiringDate)
                    .HasColumnType("datetime")
                    .IsRequired();

                entity.Property(x => x.Price)
                    .HasColumnType("decimal")
                    .IsRequired();

                entity.HasOne(x => x.PersonService)
                   .WithMany(y => y.ServicesPerson)
                   .HasForeignKey(x => x.PersonId)
                   .OnDelete(DeleteBehavior.Restrict);
                   

                entity.HasOne(x => x.NannyService)
                   .WithMany(y => y.ServicesNanny)
                   .HasForeignKey(x => x.NannyId)
                   .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Nanny>(entity =>
            {
                entity.ToTable("Nannys");

                entity.Property(x => x.ServicePrice)
                    .HasColumnType("float")
                    .IsRequired();
            });

            modelBuilder.Entity<CommentRank>(entity =>
            {
                entity.ToTable("CommentsRank");

                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(x => x.Comment)
                    .HasColumnType("varchar(50)")
                    .IsRequired(false);

                entity.HasOne(x => x.PersonWhoComment)
                    .WithMany(y => y.CommentsRank)
                    .HasForeignKey(y => y.PersonWhoCommentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.NannyWhoRecieveTheComment)
                    .WithMany(y => y.CommentsRankNanny)
                    .HasForeignKey(y => y.NannyWhoRecieveTheCommentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
