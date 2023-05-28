using Amazon.DynamoDBv2.DataModel;

namespace Common.Entities
{
    [DynamoDBTable("LicenseManagement-Entitlements")]
    public class ProductEntitlementDto
    {
        [DynamoDBHashKey]
        public Guid Id { get; private set; } = Guid.NewGuid();

        public Guid LicenseId { get; set; }

        public Guid UserId { get; set; }

        public string ProductName { get; set; } = null!;

        public string ProductDescription { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public DateTime? Until { get; set; }
    }
}
