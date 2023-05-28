namespace Common.Entities
{
    public class ProductDto
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string ProductGroup { get; set; } = null!;

        public bool Billable { get; set; }

        public List<string> Countries { get; set; } = new() { "UA" };
    }
}
