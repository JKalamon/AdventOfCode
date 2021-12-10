namespace AdventOfCode2021
{
  internal class SmokeBasinSolution : IChallenge
  {
    public string Title => "--- Day 9: Smoke Basin ---";

    public DateTime DateTime => new(2021, 12, 9);

    private IEnumerable<IEnumerable<int?>> inputFile = File.ReadAllLines("9SmokeBasinSolution/input.txt").Select(x => x.Select(y => (int?)int.Parse(y.ToString())));

    public object SolvePart1()
    {
      var matches = new List<int>();
      for (int y = 0; y < inputFile.Count(); y++)
      {
        for (int x = 0; x < inputFile.ElementAt(y).Count(); x++)
        {
          var left = inputFile.ElementAt(y).ElementAtOrDefault(x - 1) ?? 9;
          var current = inputFile.ElementAt(y).ElementAt(x) ?? 9;
          var right = inputFile.ElementAt(y).ElementAtOrDefault(x + 1) ?? 9;
          var up = (inputFile.ElementAtOrDefault(y - 1) ?? Enumerable.Empty<int?>()).ElementAtOrDefault(x) ?? 9;
          var bottom = (inputFile.ElementAtOrDefault(y + 1) ?? Enumerable.Empty<int?>()).ElementAtOrDefault(x) ?? 9;
          if (current < left && current < right && current < up && current < bottom)
          {
            matches.Add(current);
          }
        }
      }
      return matches.Select(x => x + 1).Sum();
    }

    record Point(int x, int y);

    public object? SolvePart2()
    {
      var basins = new List<int>();
      var trackedPoints = new List<Point>();

      for (int y = 0; y < inputFile.Count(); y++)
      {
        for (int x = 0; x < inputFile.ElementAt(y).Count(); x++)
        {
          if ((inputFile.ElementAt(y).ElementAt(x) ?? 9) == 9 || trackedPoints.Contains(new(x, y)))
            continue;

          var currentBasin = new List<Point>();
          var pointsToCheck = new Stack<Point>();
          pointsToCheck.Push(new Point(x, y));

          while (pointsToCheck.TryPop(out var cp))
          {
            var current = (inputFile.ElementAtOrDefault(cp.y) ?? Enumerable.Empty<int?>()).ElementAtOrDefault(cp.x) ?? 9;
            if (current == 9 || trackedPoints.Contains(new(cp.x, cp.y)))
              continue;

            pointsToCheck.Push(new Point(cp.x, cp.y - 1));
            pointsToCheck.Push(new Point(cp.x, cp.y + 1));

            currentBasin.Add(new(cp.x, cp.y));
            trackedPoints.Add(new(cp.x, cp.y));

            var tempx = cp.x - 1;

            while ((inputFile.ElementAt(cp.y).ElementAtOrDefault(tempx) ?? 9) != 9)
            {
              currentBasin.Add(new(tempx, cp.y));
              trackedPoints.Add(new(tempx, cp.y));
              pointsToCheck.Push(new Point(tempx, cp.y - 1));
              pointsToCheck.Push(new Point(tempx, cp.y + 1));

              tempx--;
            }

            tempx = cp.x + 1;
            while ((inputFile.ElementAt(cp.y).ElementAtOrDefault(tempx) ?? 9) != 9)
            {
              currentBasin.Add(new(tempx, cp.y));
              trackedPoints.Add(new(tempx, cp.y));
              pointsToCheck.Push(new Point(tempx, cp.y - 1));
              pointsToCheck.Push(new Point(tempx, cp.y + 1));

              tempx++;
            }
          }

          basins.Add(currentBasin.Count());
        }
      }

      return basins.OrderByDescending(x => x).Take(3).Aggregate(1, (x, y) => x * y);
    }
  }
}

