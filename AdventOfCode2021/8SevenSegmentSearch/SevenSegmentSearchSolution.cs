using MoreLinq;

namespace AdventOfCode2021
{
  internal class SevenSegmentSearch : IChallenge
  {
    public string Title => "--- Day 8: Seven Segment Search ---";

    public DateTime DateTime => new(2021, 12, 7);

    private IEnumerable<Signal> inputFile = File.ReadAllLines("8SevenSegmentSearch/input.txt").Select(x => new Signal(x.Split(" | ")[0].Split(" "), x.Split(" | ")[1].Split(" ")));


    public record Signal(string[] TestDigits, string[] ToBeDecoded);

    public record Digit(int Number, char[] Segments);

    public Digit[] Digits = new Digit[10]
    {
      new Digit(0, new[]{'a','b','c','e','f','g'}),
      new Digit(1, new[]{'c', 'f'}),
      new Digit(2, new[]{'a', 'c','d','e','g'}),
      new Digit(3, new[]{'a','c','d','f','g'}),
      new Digit(4, new[]{'b','c','d','f'}),
      new Digit(5, new[]{'a','b','d','f','g'}),
      new Digit(6, new[]{'a','b','d','e','f','g'}),
      new Digit(7, new[]{'a','c','f'}),
      new Digit(8, new[]{'a','b','c','d','e','f','g'}),
      new Digit(9, new[]{'a','b','c','d','f','g'}),
    };


    public object SolvePart1()
    {
      var count = 0;
      this.inputFile.ForEach(x =>
      {
        count += x.ToBeDecoded.Count(y => y.Length == Digits[1].Segments.Length);
        count += x.ToBeDecoded.Count(y => y.Length == Digits[4].Segments.Length);
        count += x.ToBeDecoded.Count(y => y.Length == Digits[7].Segments.Length);
        count += x.ToBeDecoded.Count(y => y.Length == Digits[8].Segments.Length);
      });

      return count;
    }

    public object? SolvePart2()
    {
      var count = 0;
      this.inputFile.ForEach(signal =>
      {
        var allLetters = "abcdefg";
        var possibleMapping = new Dictionary<char, List<char>>()
        {
          ['a'] = allLetters.ToList(),
          ['b'] = allLetters.ToList(),
          ['c'] = allLetters.ToList(),
          ['d'] = allLetters.ToList(),
          ['e'] = allLetters.ToList(),
          ['f'] = allLetters.ToList(),
          ['g'] = allLetters.ToList(),
        };

        var digitsGrouped = Digits.GroupBy(x => x.Segments.Length);
        var knownNumbers = new Dictionary<int, string>();
        digitsGrouped.Where(x => x.Count() == 1).ForEach(x =>
        {
          var digit = x.First();

          signal.TestDigits.Where(x => x.Length == digit.Segments.Length).ForEach(signalText =>
          {
            knownNumbers.Add(digit.Number, string.Concat(signalText.OrderBy(x => x)));

            digit.Segments.ForEach(x =>
            {
              possibleMapping[x] = possibleMapping[x].Intersect(signalText).ToList();
            });

            allLetters.Except(digit.Segments).ForEach(x =>
            {
              possibleMapping[x] = possibleMapping[x].Except(signalText).ToList();
            });
          });
        });

        string output = "";
        for (int i = 0; i < 4; i++)
        {
          var signalText = signal.ToBeDecoded[i];
          var possibleDigits = digitsGrouped.Where(x => x.Key == signalText.Length).First();

          if (possibleDigits.Count() == 1)
          {
            output += possibleDigits.Single().Number.ToString();

            continue;
          }

          //// there are two groups 0, 6, 9 and 2, 3, 5
          Digit? foundDigit = null;
          foreach (var digit in possibleDigits)
          {
            if (SearchForNumber(digit.Number, signalText, knownNumbers))
            {
              if (foundDigit != null)
              {
                throw new Exception("Two matches???");
              }
              foundDigit = digit;
            }
          }

          if (foundDigit == null)
          {
            throw new Exception("No matches");
          }

          output += foundDigit.Number.ToString();          
        }

        count += int.Parse(output);
      });

      return count;
    }

    private bool SearchForNumber(int potentialNumber, string input, Dictionary<int, string> knownNumbers)
    {
      string ao = string.Concat(input.OrderBy(c => c));
      return potentialNumber switch
      {
        0 => !ao.ContainsAll(knownNumbers[4]) && ao.ContainsAll(knownNumbers[1]),
        6 => !ao.ContainsAll(knownNumbers[1]) && !ao.ContainsAll(knownNumbers[4]) && !ao.ContainsAll(knownNumbers[7]),
        9 => ao.ContainsAll(knownNumbers[1]) && ao.ContainsAll(knownNumbers[4]),
        2 => !ao.ContainsAll(knownNumbers[1]) && !ao.ContainsAlmostAll(knownNumbers[4]),
        3 => ao.ContainsAll(knownNumbers[7]) && ao.ContainsAll(knownNumbers[1]),
        5 => ao.ContainsAlmostAll(knownNumbers[4]) && !ao.ContainsAll(knownNumbers[1]),
      };
    }
  }

  public static class Ext
  {
    public static bool ContainsAll(this string test, string anotherString)
    {
      return anotherString.All(x => test.Contains(x));
    }

    public static bool ContainsAlmostAll(this string test, string anotherString)
    {
      return anotherString.Select(x => test.Contains(x)).Count(x => x) == anotherString.Count() - 1;
    }
  }
}

