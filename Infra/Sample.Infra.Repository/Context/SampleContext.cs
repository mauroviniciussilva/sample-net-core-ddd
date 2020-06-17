using Sample.Domain.Entities;
using Sample.Infra.Data.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Sample.Infra.Data.Context
{
    /// <summary>
    /// An example of how to make contexts in the application
    /// </summary>
    public class SampleContext : DbContext
    {
        #region [ Entities of the Context ]

        internal DbSet<User> User { get; set; }
        internal DbSet<SampleEntity> Sample { get; set; }

        #endregion

        #region [ Constructor ]

        public SampleContext(DbContextOptions options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            Database.EnsureCreated();
        }

        #endregion

        #region [ Methods ]

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region [ Maps of the Context ]

            SampleMap.OnCreateTable(ref modelBuilder);
            UserMap.OnCreateTable(ref modelBuilder);

            #endregion
        }

        #endregion
    }
}