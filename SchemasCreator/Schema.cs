using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.History;

namespace SchemasCreator
{
   public static class Schema
    {
        public static void Create(string connectionString, string schemaName)
        {
            var tenantDataMigrationsConfiguration = new DbMigrationsConfiguration<DataContext>();
            tenantDataMigrationsConfiguration.AutomaticMigrationsEnabled = true;
            tenantDataMigrationsConfiguration.SetSqlGenerator("System.Data.SqlClient", new SqlServerSchemaAwareMigrationSqlGenerator(schemaName));
            tenantDataMigrationsConfiguration.SetHistoryContextFactory("System.Data.SqlClient", (existingConnection, defaultSchema) => new HistoryContext(existingConnection, schemaName));
            tenantDataMigrationsConfiguration.TargetDatabase = new System.Data.Entity.Infrastructure.DbConnectionInfo(connectionString, "System.Data.SqlClient");
            tenantDataMigrationsConfiguration.MigrationsAssembly = typeof(DataContext).Assembly;
            tenantDataMigrationsConfiguration.MigrationsNamespace = "DataContext.Migrations.TestSchema";

            DbMigrator tenantDataCtxMigrator = new DbMigrator(tenantDataMigrationsConfiguration);
            tenantDataCtxMigrator.Update();
        }
    }
}
