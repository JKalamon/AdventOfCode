using MoreLinq;

namespace AdventOfCode2021
{
  internal class DumboOctopusSolution : IChallenge
  {
    public string Title => "--- Day 11: Dumbo Octopus ---";

    public DateTime DateTime => new(2021, 12, 11);

    private int[][] inputFile = File.ReadAllLines("11DumboOctopus/input.txt").Select(x => x.Select(y => int.Parse(y.ToString())).ToArray()).ToArray();

    public record DumboOctopus(int X, int Y, int Power)
    {
      public int Power { get; set; } = Power;
    }

    public object SolvePart1()
    {
      var dumboList = new List<DumboOctopus>();

      //// flatten list
      for (int y = 0; y < inputFile.Count(); y++)
      {
        for (int x = 0; x < inputFile.ElementAt(y).Count(); x++)
        {
          dumboList.Add(new DumboOctopus(x, y, inputFile[y][x]));
        }
      }

      var flashCount = 0;
      for (int i = 0; i < 100; i++)
      {
        dumboList.ForEach(x => x.Power++);

        while (dumboList.Any(x => x.Power >= 10))
        {
          //// flash
          dumboList.Where(x => x.Power >= 10).ForEach(octopus =>
          {
            flashCount++;
            octopus.Power = 0;
            IncreaseAdjacent(dumboList, octopus.X, octopus.Y);
          });
        }
      }

      return flashCount;
    }


    void IncreaseAdjacent(List<DumboOctopus> list, int x, int y)
    {
      IncreasePower(list, x - 1, y - 1);
      IncreasePower(list, x, y - 1);
      IncreasePower(list, x + 1, y - 1);

      IncreasePower(list, x - 1, y);
      IncreasePower(list, x + 1, y);

      IncreasePower(list, x - 1, y + 1);
      IncreasePower(list, x, y + 1);
      IncreasePower(list, x + 1, y + 1);
    }

    void IncreasePower(List<DumboOctopus> list, int x, int y)
    {
      var tmp = list.FirstOrDefault(o => o.X == x && o.Y == y);
      if (tmp != null && tmp.Power != 0)
        tmp.Power++;
    }



    public object? SolvePart2()
    {
      var dumboList = new List<DumboOctopus>();

      //// flatten list
      for (int y = 0; y < inputFile.Count(); y++)
      {
        for (int x = 0; x < inputFile.ElementAt(y).Count(); x++)
        {
          dumboList.Add(new DumboOctopus(x, y, inputFile[y][x]));
        }
      }

      var stepCount = 0;
      while(!dumboList.All(x => x.Power == 0))
      {
        dumboList.ForEach(x => x.Power++);

        while (dumboList.Any(x => x.Power >= 10))
        {
          //// flash
          dumboList.Where(x => x.Power >= 10).ForEach(octopus =>
          {
            octopus.Power = 0;
            IncreaseAdjacent(dumboList, octopus.X, octopus.Y);
          });
        }

        stepCount++;
      }

      return stepCount;
    }
  }
}

