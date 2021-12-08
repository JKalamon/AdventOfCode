namespace AdventOfCode2021
{
  internal class LanternfishSolution : IChallenge
  {
    public string Title => "--- Day 6: Lanternfish ---";

    public DateTime DateTime => new(2021, 12, 6);

    private IEnumerable<int> inputFile = File.ReadAllLines("6Lanternfish/input.txt")[0].Split(',').Select(x => int.Parse(x));

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
        for (int i = days - 1; i >= 0; i -= 1)
        {
          this.TimeToBorn--;
          if(this.TimeToBorn < 0)
          {
            this.TimeToBorn = 6;
            children.Add(new LaternFish(days-i, 8));
          }
        }

        var xx = children.SelectMany(x => x.GetChildren(days - x.BirthDay)).ToList();
        children.AddRange(xx);
        return children;
      }
    }

    public object SolvePart1()
    {
      var initialGrono = this.inputFile.Select(x => new LaternFish(0, x));
      return (initialGrono.SelectMany(x => x.GetChildren(80)).Count() + initialGrono.Count()).ToString();
    }

    public object? SolvePart2()
    {
      long[] fishesGens = new long[9];
      foreach (int i in this.inputFile)
      {
        fishesGens[i]++;
      }

      for (int i = 0; i < 256; i++)
      {
        long newBorns = fishesGens[0];
        for (int j = 1; j < fishesGens.Length; j++)
        {
          fishesGens[j - 1] = fishesGens[j];
        }

        fishesGens[8] = newBorns;
        fishesGens[6] += newBorns;
      }

      return fishesGens.Sum().ToString();
    }
  }
}
