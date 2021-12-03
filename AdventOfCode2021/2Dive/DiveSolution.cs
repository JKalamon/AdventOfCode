using MoreLinq;

namespace AdventOfCode2021;

internal class DiveSolution : IChallenge
{
  public string Title => "--- Day 2: Dive! ---";

  public DateTime DateTime => new(2021, 12, 2);

  enum DiveDirection
  {
    Forward,
    Down,
    Up
  }

  record DiveCommand(DiveDirection Direction, int NumberOfUnits);

  private IEnumerable<DiveCommand> commands = File.ReadAllLines("2Dive/input.txt").Select(x => new DiveCommand(Enum.Parse<DiveDirection>(x.Split(' ')[0], true), int.Parse(x.Split(' ')[1])));

  public string SolvePart1()
  {
    int position = 0, depth = 0;

    commands.ForEach(c =>
    {
      switch (c.Direction)
      {
        case DiveDirection.Forward:
          position += c.NumberOfUnits;
          break;
        case DiveDirection.Down:
          depth += c.NumberOfUnits;
          break;
        case DiveDirection.Up:
          depth -= c.NumberOfUnits;
          break;
      }
    });

    return (position * depth).ToString();
  }

  public string SolvePart2()
  {
    int position = 0, depth = 0, aim = 0;
    commands.ForEach(c =>
    {
      switch (c.Direction)
      {
        case DiveDirection.Forward:
          position += c.NumberOfUnits;
          depth += c.NumberOfUnits * aim;
          break;
        case DiveDirection.Down:
          aim += c.NumberOfUnits;
          break;
        case DiveDirection.Up:
          aim -= c.NumberOfUnits;
          break;
      }
    });

    return (position * depth).ToString();
  }
}