using System.Text.RegularExpressions;

namespace PingPong_ApiGateway_Domain.ValueObjects
{
    public partial record Email
    {
        private const string Pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        public string Value { get; init; }
        private Email(string value) => Value = value;

        [GeneratedRegex(Pattern)]
        private static partial Regex EmailRegex();

        public static Email? Create(string value)
        {
            if (string.IsNullOrEmpty(value) || !EmailRegex().IsMatch(value))
            {
                return null;
            }

            return new Email(value);
        }
    }
}
