using LanguageExt;

namespace Elves
{
    public readonly struct Snafu : IComparable<Snafu>, IEquatable<Snafu>, IComparable
    {
        public const long MaxValue = 76293945312L;
        private const long MaxLength = 16;

        private readonly Digits _digits;

        private Snafu(Digits digits) => _digits = digits;

        public static Either<ParsingError, Snafu> Parse(string potentialSnafu)
            => ValidateLength(potentialSnafu)
                .Bind(Digits.From)
                .Filter(IsDigitsInRange)
                .Map(digits => new Snafu(digits))
                .ToEither(new ParsingError($"Invalid snafu: {potentialSnafu}"));

        private static Option<string> ValidateLength(string potentialSnafu)
            => !string.IsNullOrEmpty(potentialSnafu) && potentialSnafu.Length <= MaxLength
                ? potentialSnafu
                : Option<string>.None;

        private static bool IsDigitsInRange(Digits digits)
            => digits.ToNumber() > 0 && digits.ToNumber() <= MaxValue;

        public override string ToString() => _digits.ToString();
        public override int GetHashCode() => _digits.GetHashCode();
        public int CompareTo(Snafu other) => _digits.CompareTo(other._digits);

        public int CompareTo(object? obj)
            => obj switch
            {
                null => 1,
                Snafu snafu => CompareTo(snafu),
                _ => throw new ArgumentException("must be of type Snafu")
            };

        public static bool operator ==(Snafu snafu, Snafu other) => snafu.Equals(other);

        public static bool operator !=(Snafu snafu, Snafu other) => !(snafu == other);

        public override bool Equals(object obj) => obj is Snafu snafu && Equals(snafu);
        public bool Equals(Snafu other) => _digits.Equals(other._digits);

        public long ToNumber() => _digits.ToNumber();
    }
}