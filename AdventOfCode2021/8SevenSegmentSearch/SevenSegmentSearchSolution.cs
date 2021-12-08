using MoreLinq;

namespace AdventOfCode2021
{
  internal class SevenSegmentSearch : IChallenge
  {
    public string Title => "--- Day 8: Seven Segment Search ---";

    public DateTime DateTime => new(2021, 12, 7);

    private IEnumerable<Signal> inputFile = File.ReadAllLines("8SevenSegmentSearch/input.txt").Select(x => new Signal(x.Split(" | ")[0].Split(" "), x.Split(" | ")[1].Split(" ")));


    public record Signal(string[] Digits, string[] ToBeDecoded);

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
        var possibleOutputs = new Dictionary<char, List<char>>()
        {
          ['a'] = new() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' },
          ['b'] = new() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' },
          ['c'] = new() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' },
          ['d'] = new() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' },
          ['e'] = new() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' },
          ['f'] = new() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' },
          ['g'] = new() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' },
        };

        var digitsGrouped = Digits.GroupBy(x => x.Segments.Length);
        digitsGrouped.Where(x => x.Count() == 1).ForEach(x =>
        {
          var digit = x.First();
          
          signal.Digits.Where(x => x.Length == digit.Segments.Length).ForEach(aaa =>
          {
            digit.Segments.ForEach(x =>
            {
              possibleOutputs[x] = possibleOutputs[x].Intersect(aaa).ToList();
            });
          });          
        });

        var output = new int[4];
        signal.ToBeDecoded;
        
      });

      return count;
    }
  }
}

