using Sample.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Sample.Infra.Data.Mapping
{
    public class UserMap
    {
        public static void OnCreateTable(ref ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable(nameof(User), "dbo");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.TypeId).HasColumnType("Int").IsRequired();
                entity.Property(e => e.Name).HasColumnType("Varchar(128)").HasMaxLength(128).IsRequired();
                entity.Property(e => e.Login).HasColumnType("Varchar(128)").HasMaxLength(128).IsRequired();
                entity.Property(e => e.Password).HasColumnType("Varchar(128)").HasMaxLength(128).IsRequired();
                entity.Property(e => e.Active).HasColumnType("Bit").IsRequired();
                entity.Property(e => e.UserCreationId).IsRequired().HasColumnType("Int");
                entity.Property(e => e.CreationDate).IsRequired().HasColumnType("Smalldatetime");
                entity.Property(e => e.UserModificationId).HasColumnType("Int");
                entity.Property(e => e.ModificationDate).HasColumnType("Smalldatetime");

                entity.HasQueryFilter(b => EF.Property<bool>(b, "Active") == true);
            });
        }
    }
}
