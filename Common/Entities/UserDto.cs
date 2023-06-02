using Amazon.DynamoDBv2.DataModel;

namespace Common.Entities
{
    /// <summary>
    /// Represents model for user definition.
    /// </summary>
    [DynamoDBTable("LicenseManagement-Users")]
    public class UserDto
    {
        /// <summary>
        /// Unique-id created by the api processor.
        /// </summary>
        [DynamoDBHashKey]
        public Guid Uuid { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Unique Id that a user logs in to their system with, often an email address.
        /// </summary>
        public string Username { get; set; } = null!;

        /// <summary>
        /// Title of the user.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// First name of a user.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Last name of a user.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Phone number os a user/machine owner.
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Email address of a user/machine owner.
        /// </summary>
        public string? EmailAddress { get; set; }

        /// <summary>
        /// Preferred language services should use.
        /// </summary>
        public string PrefferedServiceLanguage { get; set; } = "en-US";

        /// <summary>
        /// User type (e.g Human, Service).
        /// </summary>
        public string Type { get; set; } = "Human";

        /// <summary>
        /// Last update date.
        /// </summary>
        public DateTimeOffset UpdatedOn { get; } = DateTimeOffset.UtcNow;
    }
}
