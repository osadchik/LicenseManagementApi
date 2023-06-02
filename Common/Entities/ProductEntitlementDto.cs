using Amazon.DynamoDBv2.DataModel;

namespace Common.Entities
{
    [DynamoDBTable("LicenseManagement-Entitlements")]
    public class ProductEntitlementDto
    {
        [DynamoDBHashKey]
        public Guid EntitlementId { get; set; } = Guid.NewGuid();

        public string LicenseId { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public string ProductName { get; set; } = null!;

        public string ProductDescription { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public DateTime? Until { get; set; }
    }
}
