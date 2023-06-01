using Amazon.DynamoDBv2.DataModel;

namespace Common.Entities
{
    [DynamoDBTable("LicenseManagement-Products")]
    public class ProductDto
    {
        [DynamoDBHashKey]
        public Guid ProductId { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string ProductGroup { get; set; } = null!;

        public bool Billable { get; set; }

        public List<string> Countries { get; set; } = new() { "UA" };
    }
}
