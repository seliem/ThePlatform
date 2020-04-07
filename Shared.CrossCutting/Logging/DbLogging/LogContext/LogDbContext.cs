

namespace Shared.CrossCutting.Logging.LogContext
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    public class LogDbContext : DbContext
    {
        public virtual DbSet<EventLog> Logs { get; set; }

        public LogDbContext(string connenctionString) : base(connenctionString)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            string DefaultSchema = ConfigurationManager.AppSettings["DefaultSchema"].ToString();
            modelBuilder.HasDefaultSchema(DefaultSchema);

        }
    }
}
