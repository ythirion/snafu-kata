using LanguageExt;

namespace Elves.Tests
{
    public static class Extensions
    {
        internal static bool Is(this Either<ParsingError, Snafu> snafu, Digits other)
            => snafu.Exists(s => s.ToString() == other.ToString());
    }
}