namespace SchemasCreator
{
    public class ContactInfo
    {
        public int Id { get; set; }
        public string Details { get; set; }

        public int PersonId { get; set; }
        public virtual Person Person { get; set; }
    }
}
