
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using Shared.Domain.Model.Auditing;

namespace Shared.Infrastucture.Data.DBContext
{
    public class SharedDbContext : DbContext
    {

        public SharedDbContext(string connectionString) : base(connectionString)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public SharedDbContext(string connectionString, bool? enableOracleNamingConvention = null) : base(connectionString)
        => EnableOracleNamingConvention = enableOracleNamingConvention;


        public DbSet<AuditTrail> AuditTrails { get; set; }
        public bool? EnableOracleNamingConvention { get; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
          .Where(type => !String.IsNullOrEmpty(type.Namespace))
          .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
          type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            if (EnableOracleNamingConvention == true)
            {

                modelBuilder.Types().Configure(c => c.ToTable(ConvertToOracleNaming(c.ClrType.Name)));

                modelBuilder.Properties().Configure(c => c.HasColumnName(ConvertToOracleNaming(c.ClrPropertyInfo.Name)));
            }
        }

        private string ConvertToOracleNaming(string objName)
        => string.Concat(objName.Select(c => char.IsUpper(c) ? $"_{c.ToString()}" : c.ToString())).TrimStart('_').ToUpper();

    }
}
