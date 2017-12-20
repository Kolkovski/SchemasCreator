using System.Collections.Generic;

namespace SchemasCreator
{
    public class Person
    {
        public Person()
        {
            ContactInfo = new List<ContactInfo>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ContactInfo> ContactInfo { get; set; }
    }
}
