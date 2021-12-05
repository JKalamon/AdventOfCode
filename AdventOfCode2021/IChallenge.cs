using System;

namespace AdventOfCode2021
{
  internal interface IChallenge
  {
    string Title { get; }

    DateTime DateTime { get; }

    string SolvePart1();

    string? SolvePart2();

  }
}

