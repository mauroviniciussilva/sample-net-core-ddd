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
                entity.ToTable(nameof(User));

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.TypeId).IsRequired().HasColumnType("Int");
                entity.Property(e => e.Name).IsRequired().HasColumnType("Varchar(128)").HasMaxLength(128);
                entity.Property(e => e.Login).IsRequired().HasColumnType("Varchar(128)").HasMaxLength(128);
                entity.Property(e => e.Password).IsRequired().HasColumnType("Varchar(128)").HasMaxLength(128);
                entity.Property(e => e.Active).IsRequired().HasColumnType("TinyInt");
                entity.Property(e => e.UserCreationId).IsRequired().HasColumnType("Int");
                entity.Property(e => e.CreationDate).IsRequired().HasColumnType("DateTime");
                entity.Property(e => e.UserModificationId).HasColumnType("Int");
                entity.Property(e => e.ModificationDate).HasColumnType("DateTime");
            });
        }
    }
}
