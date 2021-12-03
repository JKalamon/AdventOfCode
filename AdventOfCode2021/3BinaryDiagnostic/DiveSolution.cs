using MoreLinq;

namespace AdventOfCode2021;

internal class BinaryDiagnosticSolution : IChallenge
{
  public string Title => "--- Day 3: Binary Diagnostic ---";

  public DateTime DateTime => new(2021, 12, 3);

  private IEnumerable<string> commands = File.ReadAllLines("3BinaryDiagnostic/input.txt");

  public string SolvePart1()
  {
    var gammaRate = string.Empty;
    var epsilon = string.Empty;
    var numberLength = commands.First().Length;

    for (int i = 0; i < numberLength; i++)
    {
      var column = commands.Select(x => x[i]);
      gammaRate += column.Count(x => x == '1') > column.Count(x => x == '0') ? "1" : "0";
      epsilon += column.Count(x => x == '1') <= column.Count(x => x == '0') ? "1" : "0";
    }

    var gammaRateDecimal = Convert.ToInt32(gammaRate, 2);
    var epsilonDecimal = Convert.ToInt32(epsilon, 2);

    return (gammaRateDecimal * epsilonDecimal).ToString();
  }

  public string? SolvePart2()
  {
    var oxygenRatingBinary = string.Empty;
    var co2RatingBinary = string.Empty;

    var oxygenNumbers = commands;
    var coNumbers = commands;

    var numberLength = commands.First().Length;

    oxygenRatingBinary = GetNumber(true);
    co2RatingBinary = GetNumber(false);

    string GetNumber(bool mostCommon)
    {
      var list = this.commands;
      var index = 0;

      while (list.Count() > 1)
      {
        var column = list.Select(x => x[index]);
        var mostCommonCharacter = column.Count(x => x == '1') >= column.Count(x => x == '0') ? '1' : '0';
        var character = mostCommon ? mostCommonCharacter : (mostCommonCharacter == '1' ? '0' : '1');
        list = list.Where(x => x[index] == character).ToList();
        index++;
      }

      return list.First();
    }

    var gammaRateDecimal = Convert.ToInt32(oxygenRatingBinary, 2);
    var epsilonDecimal = Convert.ToInt32(co2RatingBinary, 2);

    return (gammaRateDecimal * epsilonDecimal).ToString();
  }
}