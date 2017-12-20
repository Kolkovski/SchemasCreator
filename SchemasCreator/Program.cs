namespace SchemasCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            string schemaName = "FirstSchema";
            string connectionString = @"Server=.\SQLEXPRESS;Database=TestSchema;Trusted_Connection=True;";
            Schema.Create(connectionString, schemaName);
            Schema.Create(connectionString, "SecondSchema");
            Schema.Create(connectionString, "ThirdSchema");

            using (var context = DataContext.Create(connectionString, schemaName))
            {
                context.Persons.Add(new Person { Name = "Victor", ContactInfo = { new ContactInfo { Details = ".Net Dev" }, new ContactInfo { Details = "BSUIR" } } });
                context.Persons.Add(new Person { Name = "Vladislav", ContactInfo = { new ContactInfo { Details = "Front-end Dev" }, new ContactInfo { Details = "BSU" } } });
                context.SaveChanges();
            }
        }
    }
}
