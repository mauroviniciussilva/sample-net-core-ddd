using Sample.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Sample.Infra.Data.Mapping
{
    /// <summary>
    /// Example of how to do the map of an entity
    /// </summary>
    public class SampleMap
    {
        public static void OnCreateTable(ref ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SampleEntity>(entity =>
            {
                entity.ToTable(nameof(SampleEntity), "dbo");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.UserCreationId).IsRequired().HasColumnType("Int");
                entity.Property(e => e.CreationDate).IsRequired().HasColumnType("Smalldatetime");
                entity.Property(e => e.UserModificationId).HasColumnType("Int");
                entity.Property(e => e.ModificationDate).HasColumnType("Smalldatetime");
                entity.Property(e => e.Active).IsRequired().HasColumnType("Bit");

                // Only returns registries that are Active
                entity.HasQueryFilter(b => EF.Property<bool>(b, "Active") == true);
            });
        }
    }
}