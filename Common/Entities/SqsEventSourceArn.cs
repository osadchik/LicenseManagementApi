using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Common.Entities
{
    public class SqsEventSourceArn
    {
        private const string SqsEventSourceArnPattern = @"^(arn):(aws):(sqs):[A-Za-z0-9\-:]+";

        public string Value { get; }

        public SqsEventSourceArn([RegularExpression(SqsEventSourceArnPattern)] string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            if (!Regex.IsMatch(value, SqsEventSourceArnPattern)) throw new ArgumentException($"{value} doesn't match {SqsEventSourceArnPattern} pattern.");

            Value = value;
        }

        public static bool IsMatch(string value)
        {
            return Regex.IsMatch(value, SqsEventSourceArnPattern);
        }
    }
}
