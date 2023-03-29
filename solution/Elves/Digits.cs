using System.Collections.Immutable;
using LanguageExt;

namespace Elves
{
    public readonly struct Digits : IComparable<Digits>, IEquatable<Digits>, IComparable
    {
        private readonly Seq<Digit> _value;

        private Digits(Seq<Digit> chars) => _value = chars;

        public long ToNumber()
            => _value
                .Reverse()
                .Map(digit => digit.ToInt())
                .Map((index, x) => (long) (Math.Pow(5.0, index) * x))
                .Sum();

        public static Option<Digits> From(string chars)
            => !chars.IsNull() && ContainsOnlyValidCharacters(chars.ToCharArray().ToSeq())
                ? new Digits(
                    chars.ToCharArray().Bind(c => c.ToDigit())
                        .ToSeq()
                )
                : Option<Digits>.None;

        public static Option<Digits> From(params Digit[] digits)
            => !digits.IsNull()
                ? new Digits(digits.ToSeq())
                : Option<Digits>.None;

        public static readonly Map<char, Digit> ValidCharacters =
            Enum.GetValues<Digit>()
                .ToImmutableDictionary(c => (char) c, d => d)
                .ToMap();

        private static bool ContainsOnlyValidCharacters(Seq<char> potentialSnafu)
            => !potentialSnafu.IsNull() && potentialSnafu.All(x => x.IsValid());

        public override string ToString()
            => string.Concat(_value.Map(x => x.ToChar()));

        public int CompareTo(Digits other) => _value.CompareTo(other);

        public int CompareTo(object? obj)
            => obj switch
            {
                null => 1,
                Digits digits => CompareTo(digits),
                _ => throw new ArgumentException("must be of type Digits")
            };

        public override int GetHashCode() => _value.GetHashCode();
        public bool Equals(Digits other) => ToString() == other.ToString();
        public override bool Equals(object obj) => obj is Digits digits && Equals(digits);
        public static bool operator ==(Digits left, Digits right) => left.Equals(right);
        public static bool operator !=(Digits left, Digits right) => !(left == right);
    }
}