namespace AdventOfCode2020
{
  internal static class ReportRepairSolution
  {
    public static void Solve()
    {
      var inputList = File.ReadAllLines("1ReportRepair/input.txt").Select(x => int.Parse(x.Trim()));
      var x = inputList.First(x => inputList.Any(y => y + x == 2020));
      var y = 2020 - x;
      Console.WriteLine($"First part pair: {x}, {y}");
      Console.WriteLine($"First part answer: {x * y}");

      x = inputList.First(x => inputList.Any(z => inputList.Any(y => y + x + z == 2020)));
      var z = inputList.First(z => inputList.Any(y => y + x + z == 2020));
      y = 2020 - x - z;
      Console.WriteLine($"Second part pair: {x}, {y}, {z}");
      Console.WriteLine($"Second part answer: {x * y * z}");
    }
  }
}
