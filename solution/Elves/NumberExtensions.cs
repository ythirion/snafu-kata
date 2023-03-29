using LanguageExt;
using static LanguageExt.Prelude;

namespace Elves
{
    public static class NumberExtensions
    {
        public static Either<ParsingError, Snafu> ToSnafu(this long number)
            => number > 0
                ? ParseSafely(number)
                : Left(new ParsingError("number must be greater than 0"));

        private static Either<ParsingError, Snafu> ParseSafely(long number)
            => Snafu.Parse(
                string.Concat(
                    ConvertRecursively(number, new List<long>())
                        .Map(ToSymbol)
                        .Reverse()
                ));

        private static List<long> ConvertRecursively(this long number, List<long> digits)
        {
            var remaining = 0;
            var digit = number % 5;

            if (digit >= 3) remaining = 5;

            var result = (number + remaining) / 5;
            digits.Add(digit);

            return result == 0
                ? digits
                : ConvertRecursively(result, digits);
        }

        private static char ToSymbol(long digit)
            => digit switch
            {
                3L => '=',
                4L => '-',
                _ => digit.ToString().Head()
            };
    }
}