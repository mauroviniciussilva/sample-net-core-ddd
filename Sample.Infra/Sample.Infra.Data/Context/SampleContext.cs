using Sample.Domain.Entities;
using Sample.Infra.Data.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using Sample.Infra.Data.Utils;

namespace Sample.Infra.Data.Context
{
    /// <summary>
    /// The core contexto of the application
    /// </summary>
    public class CoreContext : DbContext
    {
        #region [ Entities of the Context ]

        internal DbSet<User> User { get; set; }

        #endregion

        #region [ Constructor ]

        public CoreContext(DbContextOptions options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            InitializeDataBase.Initialize(this);
        }

        #endregion

        #region [ Methods ]

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region [ Maps of the Context ]

            UserMap.OnCreateTable(ref modelBuilder);

            #endregion
        }

        #endregion
    }
}