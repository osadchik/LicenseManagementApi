using Amazon.DynamoDBv2.DataModel;

namespace Common.Entities
{
    /// <summary>
    /// Represents license definition.
    /// </summary>
    [DynamoDBTable("LicenseManagement-Licenses")]
    public class LicenseDto
    {
        private string? currency;

        /// <summary>
        /// License's unique indentifier.
        /// </summary>
        [DynamoDBHashKey]
        public Guid Id { get; } = Guid.NewGuid();

        /// <summary>
        /// Id of a product this license is assigned to.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// License cost value.
        /// </summary>
        public decimal PriceAmount { get; set; }

        /// <summary>
        /// Currency used to pay for license.
        /// </summary>
        public string Currency 
        {
            get => currency ?? PriceAmount.ToString("C");
            set => currency = value;
        }
    }
}
