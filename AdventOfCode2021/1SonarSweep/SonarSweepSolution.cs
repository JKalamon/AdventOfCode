namespace AdventOfCode2021;

internal class SonarSweepSolution : IChallenge
{
  IEnumerable<int> inputList;

  public SonarSweepSolution()
  {
    inputList = File.ReadAllLines("1SonarSweep/input.txt").Select(x => int.Parse(x.Trim()));
  }

  public string Title => "--- Day 1: Sonar Sweep ---";

  public DateTime DateTime => new(2021, 12, 1);

  public string SolvePart1()
  {
    var count = 0;
    inputList.Aggregate(int.MaxValue, (x, y) =>
    {
      if (y > x)
        count++;

      return y;
    });

    return count.ToString();
  }

  public string SolvePart2()
  {
    //// this will create an array +2 items more than expected but last itmes will get the same value as last value
    var tripleSumInput = inputList.Select((x, index) => x + inputList.ElementAtOrDefault(index + 1) + inputList.ElementAtOrDefault(index + 2));

    var count = 0;
    tripleSumInput.Aggregate(int.MaxValue, (x, y) =>
    {
      if (y > x)
        count++;

      return y;
    });

    return count.ToString();
  }
}
