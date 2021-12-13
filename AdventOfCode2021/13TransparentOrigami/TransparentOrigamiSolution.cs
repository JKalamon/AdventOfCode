using MoreLinq;

namespace AdventOfCode2021
{
  internal class TransparentOrigamiSolution : IChallenge
  {
    public string Title => "--- Day 13: Passage Pathing ---";

    public DateTime DateTime => new(2021, 12, 13);

    private IEnumerable<string> inputFile = File.ReadAllLines("13TransparentOrigami/input.txt");

    record Dot(int X, int Y)
    {
      public int X { get; set; } = X;

      public int Y { get; set; } = Y;
    }

    enum Direction
    {
      Horizontal,
      Vertical,
    }

    record Fold(Direction FoldDirection, int ColRowNumber);

    private IEnumerable<Dot> dots => inputFile.Where(x => x.Contains(',')).Select(x => new Dot(int.Parse(x.Split(',')[0]), int.Parse(x.Split(',')[1])));

    private IEnumerable<Fold> foldIntructions => inputFile.Where(x => x.Contains("fold")).Select(x => new Fold(x.Contains('y') ? Direction.Horizontal : Direction.Vertical, int.Parse(x.Split('=')[1])));

    public object SolvePart1()
    {
      var dotsList = dots.ToList();
      foreach (var instruction in foldIntructions.Take(1))
      {
        switch (instruction.FoldDirection)
        {
          case Direction.Horizontal:
            dotsList.Where(dot => dot.Y > instruction.ColRowNumber).ForEach(a => a.Y = instruction.ColRowNumber - (a.Y - instruction.ColRowNumber));
            break;

          case Direction.Vertical:
            dotsList.Where(dot => dot.X > instruction.ColRowNumber).ForEach(a => a.X = instruction.ColRowNumber - (a.X - instruction.ColRowNumber));
            break;
        }

        dotsList = dotsList.Distinct().ToList();
      }

      return dotsList.Count;
    }

    public object? SolvePart2()
    {
      var dotsList = dots.ToList();
      foreach (var instruction in foldIntructions)
      {
        switch (instruction.FoldDirection)
        {
          case Direction.Horizontal:
            dotsList.Where(dot => dot.Y > instruction.ColRowNumber).ForEach(a => a.Y = instruction.ColRowNumber - (a.Y - instruction.ColRowNumber));
            break;

          case Direction.Vertical:
            dotsList.Where(dot => dot.X > instruction.ColRowNumber).ForEach(a => a.X = instruction.ColRowNumber - (a.X - instruction.ColRowNumber));
            break;
        }

        dotsList = dotsList.Distinct().ToList();
      }

      Console.Clear();
      dotsList.ForEach(x =>
      {
        Console.SetCursorPosition(x.X, x.Y);
        Console.Write("#");
      });

      return null;
    }
  }
}

