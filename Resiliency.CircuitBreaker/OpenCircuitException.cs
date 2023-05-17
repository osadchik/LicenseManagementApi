using System.Runtime.Serialization;

namespace Resiliency.CircuitBreaker
{
    /// <summary>
    /// Signals the upstream code that circuit is open and execution isn't allowed.
    /// </summary>
    [Serializable]
    internal class OpenCircuitException : Exception
    {
        private const int DefaultBreakDurationInMinutes = 5;

        /// <summary>
        /// Default is 5 minutes from <see cref="DateTimeOffset.UtcNow"/>
        /// </summary>
        private DateTimeOffset OpenUntil { get; } = DateTimeOffset.UtcNow.AddMinutes(DefaultBreakDurationInMinutes);

        public OpenCircuitException(DateTimeOffset openUntil)
        {
            OpenUntil = openUntil;
        }

        public OpenCircuitException() : base("Circuit is open.") { }

        public OpenCircuitException(Exception inner) : base("Circuit is open.", inner) { }

        protected OpenCircuitException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            OpenUntil = info.GetDateTime("OpenUntil");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            base.GetObjectData(info, context);
            info.AddValue("OpenUntil", OpenUntil.UtcDateTime, typeof(DateTime));
        }
    }
}