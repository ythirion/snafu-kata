using LanguageExt;

namespace Elves
{
    public readonly struct Snafu : IComparable<Snafu>, IEquatable<Snafu>, IComparable
    {
        public const long MaxValue = 76293945312L;

        private readonly Digits _digits;

        private Snafu(Digits digits) => _digits = digits;

        public static Either<ParsingError, Snafu> Parse(string potentialSnafu)
            => Digits.From(potentialSnafu.ToArray())
                .Filter(digits => digits.ToNumber() > 0 && digits.ToNumber() <= MaxValue)
                .Map(digits => new Snafu(digits))
                .ToEither(new ParsingError($"Invalid snafu: {potentialSnafu}"));

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