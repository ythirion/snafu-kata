using LanguageExt;
using static Elves.Digits;

namespace Elves
{
    public enum Digit
    {
        Zero = '0',
        One = '1',
        Two = '2',
        Minus = '-',
        Equal = '='
    }

    public static class DigitExtensions
    {
        public static bool IsValid(this char c)
            => ValidCharacters.Exists((x, digit) => x == c);

        private static Digit ToDigitSafely(this char c)
            => ValidCharacters[c];

        public static Option<Digit> ToDigit(this char c)
            => c.IsValid()
                ? c.ToDigitSafely()
                : Option<Digit>.None;

        public static char ToChar(this Digit digit)
            => ValidCharacters.Filter((_, value) => value == digit)
                .Single()
                .Key;

        public static int ToInt(this Digit digit)
            => digit switch
            {
                Digit.Minus => -1,
                Digit.Equal => -2,
                _ => int.Parse(digit.ToChar().ToString())
            };
    }
}