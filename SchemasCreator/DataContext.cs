using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace SchemasCreator
{
     public class DataContext : DbContext, IDbModelCacheKeyProvider
    {
        #region Construction

        public static DataContext Create(string connectionString, string schemaName)
        {
            return new DataContext(connectionString, schemaName);
        }

        public DataContext()
        {
            Database.SetInitializer<DataContext>(null);
        }

        internal DataContext(string connectionString, string schemaName)
            : base(connectionString)
        {
            Database.SetInitializer<DataContext>(null);
            this.SchemaName = schemaName;
        }

        public string SchemaName { get; private set; }

        #endregion

        #region DataSet Properties

        public DbSet<Person> Persons { get { return this.Set<Person>(); } }
        public DbSet<ContactInfo> ContactInfos { get { return this.Set<ContactInfo>(); } }

        #endregion

        #region Overrides

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (this.SchemaName != null)
            {
                modelBuilder.HasDefaultSchema(this.SchemaName);
            }

            base.OnModelCreating(modelBuilder);
        }

        #endregion

        #region IDbModelCacheKeyProvider Members

        public string CacheKey
        {
            get { return this.SchemaName; }
        }

        #endregion
    }
}
