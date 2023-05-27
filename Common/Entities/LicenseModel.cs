namespace Common.Entities
{
    /// <summary>
    /// License model for create request.
    /// </summary>
    public class LicenseModel
    {
        /// <summary>
        /// License cost value.
        /// </summary>
        public decimal PriceAmount { get; set; }

        /// <summary>
        /// Currency used to pay for license.
        /// </summary>
        public string? Currency { get; set; }
    }
}
