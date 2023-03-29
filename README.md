# Snafu Kata
Solves AOC 2022 with Types-Driven Development and Property-Based Testing

## Original exercise
This kata comes from the [`advent of code` 2022](https://adventofcode.com/2022/day/25).

## Goal
> Design a system that can use `Snafu` numbers and convert it back to `Decimal`.

Instead of using digits, the digits are `2`, `1`, `0`, `minus` (written `-`), and `double-minus` (written `=`). 
`Minus` is worth `-1`, and `double-minus` is worth `-2`.

Because `ten` (in normal numbers) is `two fives` and `no ones`, in `SNAFU` it is written `20`. 
Since `eight` (in normal numbers) is `two fives minus two ones`, it is written `2=`.

You can do it the other direction, say you have the SNAFU number `2=-01`:
- That's `2` in the `625s` place
- `=` (double-minus) in the `125s` place
- `-` (minus) in the `25s` place
- `0` in the `5s` place
- `1` in the `1s` place
> (`2` x `625`) + (`-2` x `125`) + (`-1` x `25`) + (`0` x `5`) + (`1` x `1`) = 1250 + -250 + -25 + 0 + 1 = 976!

| Decimal    | SNAFU         |
|------------|---------------|
| 1          | 1             |
| 2          | 2             |
| 3          | 1=            |
| 4          | 1-            |
| 5          | 10            |
| 6          | 11            |
| 7          | 12            |
| 8          | 2=            |
| 9          | 2-            |
| 10         | 20            |
| 15         | 1=0           |
| 20         | 1-0           |
| 2022       | 1=11-2        |
| 12345      | 1-0---0       |
| 314159265  | 1121-1110-1=0 |

Based on this process, here are some examples of `Snafu` counterparts in `decimal`:

| SNAFU   | Decimal |
|---------|---------|
| 1=-0-2  | 1747    |
| 12111   | 906     |
| 2=0=    | 198     |
| 21      | 11      |
| 2=01    | 201     |
| 111     | 31      |
| 20012   | 1257    |
| 112     | 32      |
| 1=-1=   | 353     |
| 1-12    | 107     |
| 12      | 7       |
| 1=      | 3       |
| 122     | 37      |

Minimum value for `Snafu` is `1` and its maximum value is `2222222222222222`.

## Kata
- Apply ["Parse Don't Validate"](https://xtrem-tdd.netlify.app/Flavours/parse-dont-validate) principle
- Use ["Property-Driven Development"](https://xtrem-tdd.netlify.app/Flavours/pbt) a.k.a guiding the design of your types from `properties` using your favorite programming language and a `Property-Based Testing` library

### How to
- Start with a `parser` that always returns `Right<Snafu>`
    - Write a minimalist data structure first (empty one)
    - Your parser may look like this: `String -> Either<ParsingError, Snafu>`
- Write a `positive property` checking valid snafu can be round-tripped
    - Round-tripping: `Snafu -> String -> Snafu`
        - Assert that round-tripped `Snafu` equals original `Snafu`
    - To do so, you will have to create your own valid `Snafu` generator for `PBT`

```text
// RoundTripping Snafu : `Snafu -> String -> Snafu`
for all (validSnafu)
parseSnafu(validSnafu.ToString()) == validSnafu
```

- Imagine `other properties` and implement them

> Use the identified properties to guide your implementation

### Other properties (spoil)
You can design other properties to go further:

```text
// Parsing an invalid snafu : `Invalid String -> ParsingError`
for all (invalidSnafu)
parseSnafu(invalidSnafu.ToString()) == left(ParsingError)
```

```text
// Parsing a negative snafu : `negative snafu -> ParsingError`
for all (invalidSnafu)
parseSnafu(invalidSnafu.ToString()) == left(ParsingError)
```

```text
// RoundTripping from decimal : `long -> Snafu -> long`
for all (validLong)
toNumber(toSnafu(validLong)) == validLong
```

```text
// Decimals lower or equal 0 are not valid snafus : `long -> ParsingError`
for all (invalidLong)
toSnafu(validLong) == left(ParsingError)
```

```text
// Decimals greater than maximum value : `long -> ParsingError`
for all (greaterThanMaxValueLong)
toSnafu(greaterThanMaxValueLong) == left(ParsingError)
```

```text
// Snafu greater than maximum value : `String -> ParsingError`
for all (greaterThanMaxValueString)
toSnafu(greaterThanMaxValueString) == left(ParsingError)
```

### "Solution"
A proposal of solution is available in `C#` with `FsCheck` and `LanguageExt` in the `solution` folder.