using Common.Entities;

namespace Common.Mappers
{
    public static class LicenseMapper
    {
        public static LicenseDto MapToDto(this LicenseCreateModel licenseModel, Guid productId)
        {
            var licenseDto = new LicenseDto
            {
                ProductId = productId,
                PriceAmount = licenseModel.PriceAmount,
                Currency = licenseModel.Currency ?? "$"
            };

            return licenseDto;
        }
    }
}
