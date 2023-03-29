using FsCheck;
using FsCheck.Xunit;
using LanguageExt.UnsafeValueAccess;
using static System.Math;

namespace Elves.Tests
{
    public class SnafuProperties
    {
        private static readonly Arbitrary<Digits> _digitsGenerator =
        (
            from numberOfDigits in Gen.Choose(1, 16)
            from digits in Gen.ListOf(numberOfDigits, Arb.Generate<Digit>())
            select Digits.From(digits.ToArray()).ValueUnsafe()
        ).ToArbitrary();

        private static readonly Arbitrary<Digits> _validDigitsForSnafuGenerator =
            _digitsGenerator.Filter(digits => digits.ToNumber() > 0);

        [Property]
        public Property RoundTrippingToString() =>
            Prop.ForAll(_validDigitsForSnafuGenerator,
                digits => Snafu.Parse(digits.ToString())
                    .Is(digits)
            );

        private static readonly Arbitrary<Digits> _invalidDigitsForSnafuGenerator =
            _digitsGenerator.Filter(digits => digits.ToNumber() <= 0);

        [Property]
        public Property InvalidSnafuAreNotSnafu() =>
            Prop.ForAll(_invalidDigitsForSnafuGenerator,
                digits => Snafu.Parse(digits.ToString())
                    .IsLeft
            );

        private static readonly Arbitrary<int> _nonZeroPositiveInt =
            Gen.Choose(1, int.MaxValue)
                .ToArbitrary();

        [Property]
        public Property RoundTrippingToNumber() =>
            Prop.ForAll(_nonZeroPositiveInt,
                number => ((long) number)
                    .ToSnafu()
                    .ValueUnsafe()
                    .ToNumber() == number
            );

        private static readonly Arbitrary<int> _negativeOrZeroLong =
            Gen.Choose(0, -int.MaxValue)
                .ToArbitrary();

        [Property]
        public Property NegativeOrZeroAreNotSnafu() =>
            Prop.ForAll(_negativeOrZeroLong,
                number => ((long) number)
                    .ToSnafu()
                    .IsLeft
            );

        private static readonly Arbitrary<long> _greaterThanMaxValueLong =
            Arb.Default.Int64()
                .Filter(x => x != 0)
                .MapFilter(x => Abs(x) + Snafu.MaxValue, x => true);

        [Property]
        public Property GreaterThanMaxValueAreNotSnafu() =>
            Prop.ForAll(_greaterThanMaxValueLong,
                number => number
                    .ToSnafu()
                    .IsLeft
            );
    }
}