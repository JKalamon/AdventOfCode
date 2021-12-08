using MoreLinq;

namespace AdventOfCode2021
{
  internal class TemplateSolution : IChallenge
  {
    public string Title => "--- Day 0: Oh yeah this is a template solution!---";

    public DateTime DateTime => new(2021, 12, 3);

    private IEnumerable<string> inputFile = File.ReadAllLines("Template/input.txt");

    public object SolvePart1()
    {
      return "This is not a solution yet";
    }

    public object? SolvePart2()
    {
      return null;
    }
  }
}
