namespace AdventOfCode2021
{
  internal static class SonarSweepSolution
  {
    public static void Solve()
    {
      var inputList = File.ReadAllLines("1SonarSweep/input.txt").Select(x => int.Parse(x.Trim()));
      var count = 0;
      inputList.Aggregate(int.MaxValue, (x, y) =>
      {
        if (y > x)
          count++;

        return y;
      });

      Console.WriteLine($"Solution First Part {count}");

      count = 0;
      var tripleSumInput = inputList.Select((x, index) =>
      {
        var second = inputList.ElementAtOrDefault(index + 1);
        var third = inputList.ElementAtOrDefault(index + 2);
        if (second == 0)
          second = inputList.ElementAtOrDefault(index - 1);

        if (third == 0)
          third = inputList.ElementAtOrDefault(index - 2);
        return x + second + third;
      });

      tripleSumInput = tripleSumInput.ToArray()[0..^2];

      tripleSumInput.Aggregate(int.MaxValue, (x, y) =>
      {
        if (y > x)
          count++;

        return y;
      });

      Console.WriteLine($"Solution Second Part {count}");
    }
  }
}
