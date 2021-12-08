using MoreLinq;

namespace AdventOfCode2021
{
  internal class TheTreacheryOfWhalesSolution : IChallenge
  {
    public string Title => "--- Day 7: The Treachery of Whales ---";

    public DateTime DateTime => new(2021, 12, 7);

    private IEnumerable<int> inputFile = File.ReadAllLines("7TheTreacheryOfWhales/input.txt")[0].Split(',').Select(x => int.Parse(x));

    public class LaternFish
    {
      public int TimeToBorn { get; set; }

      public int BirthDay { get; set; }

      public LaternFish(int birthDay = 0, int timeToBorn = 8)
      {
        this.TimeToBorn = timeToBorn;
        this.BirthDay = birthDay;
      }

      public IEnumerable<LaternFish> GetChildren(int days)
      {
        var dayToBorn = 6;
        if (days < TimeToBorn)
          return Enumerable.Empty<LaternFish>();


        var children = new List<LaternFish>();
        for (int i = days - TimeToBorn - 1; i > 0; i -= dayToBorn)
        {
          children.Add(new LaternFish(days-i, 8));
        }

        var xx = children.SelectMany(x => x.GetChildren(days - x.BirthDay)).ToList();
        children.AddRange(xx);
        return children;
      }
    }

    public object SolvePart1()
    {
      var xx = Math.Round(this.inputFile.Average());

      var fuel = int.MaxValue;
      for (var tryit = xx - 1000; tryit < xx + 1000; tryit++)
      {
        var minFuel = 0;
        this.inputFile.ForEach(x =>
        {
          minFuel += (int)Math.Abs(tryit - x);
        });

        if(minFuel < fuel)
        {
          fuel = minFuel;
        }
      }

      return fuel.ToString();
    }

    public record Crab(int Position)
    {
      public int MoveCost { get; set; } = 1;
    }

    public object? SolvePart2()
    {
      var xx = Math.Round(this.inputFile.Average());

      var fuel = int.MaxValue;
      for (var tryit = xx - 1000; tryit < xx + 1000; tryit++)
      {
        var crabs = this.inputFile.Select(x => new Crab(x));
        var minFuel = 0;
        this.inputFile.ForEach(x =>
        {
          var difference = (int)Math.Abs(tryit - x);
          minFuel += Enumerable.Range(1, difference).Sum();
        });

        if (minFuel < fuel)
        {
          fuel = minFuel;
        }
      }

      return fuel.ToString();
    }

    decimal Median(int[] xs)
    {
      Array.Sort(xs);
      return xs[xs.Length / 2];
    }
  }
}
