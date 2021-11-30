using MoreLinq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
  internal static class PasswordPhilosophySolution
  {
    public static void Solve()
    {
      var regex = new Regex("(\\d+)-(\\d+) ([a-z]): ([a-z]+)");
      var inputList = File.ReadAllLines("2PasswordPhilosophy/input.txt");
      var count = 0;
      var countPart2 = 0;
      inputList.ForEach(x =>
      {
        var match = regex.Match(x);
        var min = int.Parse(match.Groups[1].Value);
        var max = int.Parse(match.Groups[2].Value);
        var letter = match.Groups[3].Value[0];
        var password = match.Groups[4].Value;
        if (password.Count(x => x == letter) >= min && password.Count(x => x == letter) <= max)
          count++;

        if (password.Char(min) == letter && password.Char(max) != letter)
          countPart2++;
        else if (password.Char(max) == letter && password.Char(min) != letter)
          countPart2++;
      });

      Console.WriteLine($"Solution First Part {count}");
      Console.WriteLine($"Solution First Part {countPart2}");
    }

    private static char Char(this string input, int num)
      => input.Length > num - 2 ? input[num - 1] : '*';
  }
}
