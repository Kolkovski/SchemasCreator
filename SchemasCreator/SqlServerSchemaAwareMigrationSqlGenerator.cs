using System;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.SqlServer;
using System.Linq;

namespace SchemasCreator
{
    public class SqlServerSchemaAwareMigrationSqlGenerator : SqlServerMigrationSqlGenerator
    {
        private string _schema;

        public SqlServerSchemaAwareMigrationSqlGenerator(string schema)
        {
            _schema = schema;
        }

        protected override void Generate(AddColumnOperation addColumnOperation)
        {
            string newTableName = _GetNameWithReplacedSchema(addColumnOperation.Table);
            var newAddColumnOperation = new AddColumnOperation(newTableName, addColumnOperation.Column, addColumnOperation.AnonymousArguments);
            base.Generate(newAddColumnOperation);
        }

        protected override void Generate(AddPrimaryKeyOperation addPrimaryKeyOperation)
        {
            addPrimaryKeyOperation.Table = _GetNameWithReplacedSchema(addPrimaryKeyOperation.Table);
            base.Generate(addPrimaryKeyOperation);
        }

        protected override void Generate(AlterColumnOperation alterColumnOperation)
        {
            string tableName = _GetNameWithReplacedSchema(alterColumnOperation.Table);
            var newAlterColumnOperation = new AlterColumnOperation(tableName, alterColumnOperation.Column, alterColumnOperation.IsDestructiveChange);
            base.Generate(newAlterColumnOperation);
        }

        protected override void Generate(DropPrimaryKeyOperation dropPrimaryKeyOperation)
        {
            dropPrimaryKeyOperation.Table = _GetNameWithReplacedSchema(dropPrimaryKeyOperation.Table);
            base.Generate(dropPrimaryKeyOperation);
        }

        protected override void Generate(CreateIndexOperation createIndexOperation)
        {
            string name = _GetNameWithReplacedSchema(createIndexOperation.Table);
            createIndexOperation.Table = name;
            base.Generate(createIndexOperation);
        }

        protected override void Generate(CreateTableOperation createTableOperation)
        {
            string newTableName = _GetNameWithReplacedSchema(createTableOperation.Name);
            var newCreateTableOperation = new CreateTableOperation(newTableName, createTableOperation.AnonymousArguments);
            newCreateTableOperation.PrimaryKey = createTableOperation.PrimaryKey;
            foreach (var column in createTableOperation.Columns)
            {
                newCreateTableOperation.Columns.Add(column);
            }

            base.Generate(newCreateTableOperation);
        }

        protected override void Generate(RenameTableOperation renameTableOperation)
        {
            string oldName = _GetNameWithReplacedSchema(renameTableOperation.Name);
            string newName = renameTableOperation.NewName.Split(new char[] { '.' }).Last();
            var newRenameTableOperation = new RenameTableOperation(oldName, newName, renameTableOperation.AnonymousArguments);
            base.Generate(newRenameTableOperation);
        }

        protected override void Generate(RenameIndexOperation renameIndexOperation)
        {
            string tableName = _GetNameWithReplacedSchema(renameIndexOperation.Table);
            var newRenameIndexOperation = new RenameIndexOperation(tableName, renameIndexOperation.Name, renameIndexOperation.NewName);
            base.Generate(newRenameIndexOperation);
        }

        protected override void Generate(AddForeignKeyOperation addForeignKeyOperation)
        {
            addForeignKeyOperation.DependentTable = _GetNameWithReplacedSchema(addForeignKeyOperation.DependentTable);
            addForeignKeyOperation.PrincipalTable = _GetNameWithReplacedSchema(addForeignKeyOperation.PrincipalTable);
            base.Generate(addForeignKeyOperation);
        }

        protected override void Generate(DropColumnOperation dropColumnOperation)
        {
            string newTableName = _GetNameWithReplacedSchema(dropColumnOperation.Table);
            var newDropColumnOperation = new DropColumnOperation(newTableName, dropColumnOperation.Name, dropColumnOperation.AnonymousArguments);
            base.Generate(newDropColumnOperation);
        }

        protected override void Generate(RenameColumnOperation renameColumnOperation)
        {
            string newTableName = _GetNameWithReplacedSchema(renameColumnOperation.Table);
            var newRenameColumnOperation = new RenameColumnOperation(newTableName, renameColumnOperation.Name, renameColumnOperation.NewName);
            base.Generate(newRenameColumnOperation);
        }

        protected override void Generate(DropTableOperation dropTableOperation)
        {
            string newTableName = _GetNameWithReplacedSchema(dropTableOperation.Name);
            var newDropTableOperation = new DropTableOperation(newTableName, dropTableOperation.AnonymousArguments);
            base.Generate(newDropTableOperation);
        }

        protected override void Generate(DropForeignKeyOperation dropForeignKeyOperation)
        {
            dropForeignKeyOperation.PrincipalTable = _GetNameWithReplacedSchema(dropForeignKeyOperation.PrincipalTable);
            dropForeignKeyOperation.DependentTable = _GetNameWithReplacedSchema(dropForeignKeyOperation.DependentTable);
            base.Generate(dropForeignKeyOperation);
        }

        protected override void Generate(DropIndexOperation dropIndexOperation)
        {
            dropIndexOperation.Table = _GetNameWithReplacedSchema(dropIndexOperation.Table);
            base.Generate(dropIndexOperation);
        }

        private string _GetNameWithReplacedSchema(string name)
        {
            string[] nameParts = name.Split('.');
            string newName;

            switch (nameParts.Length)
            {
                case 1:
                    newName = string.Format("{0}.{1}", _schema, nameParts[0]);
                    break;

                case 2:
                    newName = string.Format("{0}.{1}", _schema, nameParts[1]);
                    break;

                case 3:
                    newName = string.Format("{0}.{1}.{2}", _schema, nameParts[1], nameParts[2]);
                    break;

                default:
                    throw new NotSupportedException();
            }

            return newName;
        }
    }
}