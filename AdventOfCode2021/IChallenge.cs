namespace AdventOfCode2021
{
  internal interface IChallenge
  {
    string Title { get; }

    DateTime DateTime { get; }

    object SolvePart1();

    object? SolvePart2();

  }
}

