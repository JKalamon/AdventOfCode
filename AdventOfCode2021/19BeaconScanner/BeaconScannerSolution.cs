using MoreLinq;

namespace AdventOfCode2021
{
  internal class BeaconScannerSolution : IChallenge
  {
    public string Title => "--- Day 19: Beacon Scanner ---";

    public DateTime DateTime => new(2021, 12, 19);

    private string[] commands = File.ReadAllLines("19BeaconScanner/input.txt");

    enum Axis
    {
      X,
      InvertedX,
      Y,
      InvertedY,
      Z,
      InvertedZ,
    }

    record Direction(Axis X, Axis Y, Axis Z)
    {
      public static Direction[] AllDirections()
      {
        return new Direction[]
        {
          new Direction(Axis.X, Axis.Y, Axis.Z),
          new Direction(Axis.X, Axis.Z, Axis.InvertedY),
          new Direction(Axis.X, Axis.InvertedY, Axis.InvertedZ),
          new Direction(Axis.X, Axis.InvertedZ, Axis.Y),

          new Direction(Axis.Y, Axis.InvertedX, Axis.Z),
          new Direction(Axis.Y, Axis.Z, Axis.X),
          new Direction(Axis.Y, Axis.X, Axis.InvertedZ),
          new Direction(Axis.Y, Axis.InvertedZ, Axis.InvertedX),

          new Direction(Axis.InvertedX, Axis.InvertedY, Axis.Z),
          new Direction(Axis.InvertedX, Axis.InvertedZ, Axis.InvertedY),
          new Direction(Axis.InvertedX, Axis.Y, Axis.InvertedZ),
          new Direction(Axis.InvertedX, Axis.Z, Axis.Y),

          new Direction(Axis.InvertedY, Axis.X, Axis.Z),
          new Direction(Axis.InvertedY, Axis.InvertedZ, Axis.X),
          new Direction(Axis.InvertedY, Axis.InvertedX, Axis.InvertedZ),
          new Direction(Axis.InvertedY, Axis.Z, Axis.InvertedX),

          new Direction(Axis.Z, Axis.Y, Axis.X),
          new Direction(Axis.Z, Axis.X, Axis.Y),
          new Direction(Axis.Z, Axis.InvertedY, Axis.X),
          new Direction(Axis.Z, Axis.InvertedX, Axis.InvertedY),

          new Direction(Axis.InvertedZ, Axis.InvertedY, Axis.InvertedX),
          new Direction(Axis.InvertedZ, Axis.InvertedX, Axis.Y),
          new Direction(Axis.InvertedZ, Axis.Y, Axis.X),
          new Direction(Axis.InvertedZ, Axis.X, Axis.InvertedY),
        };
      }
    }

    record Scanner(List<Point> Beacons)
    {
      public IEnumerable<Pair> GetPairs()
      {
        var pairs = new List<Pair>();
        for (int i = 0; i < this.Beacons.Count; i++)
        {
          for (int j = 0; j < this.Beacons.Count; j++)
          {
            var p1 = this.Beacons[i];
            var p2 = this.Beacons[j];
            var pair = Pair.CreatePoint(p1, p2);
            if (p1 != p2 && !pairs.Contains(pair))
              pairs.Add(pair);
          }
        }

        return pairs;
      }

      public Point? Position { get; set; }

      public Direction? Direction { get; set; }
    }

    public record Pair(Point X, Point Y)
    {
      public static Pair CreatePoint(Point px, Point py)
      {
        if (px.X < py.X)
          return new Pair(px, py);

        if (px.X == py.X && px.Y < py.Y)
          return new Pair(px, py);

        if (px.X == py.X && px.Y == py.Y && px.Z < py.Z)
          return new Pair(px, py);

        return new Pair(py, px);
      }

      public bool ContainsPoint(Point p) => this.X == p || this.Y == p;
      public double Distance { get; set; } = X.DistanceFromPoint(Y);
    }


    public record Point(int X, int Y, int Z, int Parent)
    {
      public double DistanceFromPoint(Point b)
      {
        return Math.Pow(this.X - b.X, 2) + Math.Pow(this.Y - b.Y, 2) + Math.Pow(this.Z - b.Z, 2);
      }

      public bool Counted { get; set; } = false;
    }

    public object SolvePart1()
    {
      //// 6 matches
      ////var numberOfPoints = 6;
      ////var requiredMatches = 15;

      ////// 12 matches
      var numberOfPoints = 12;
      var requiredMatches = 66;
      var scanners = ParseInput();
      var pair = new List<Pair>();
      for (int i = 0; i < numberOfPoints; i++)
        for (int j = 0; j < numberOfPoints; j++)
        {
          if (j != i)
            pair.Add(Pair.CreatePoint(new Point(i, 0, 0, 0), new Point(j, 0, 0, 1)));
        }

      var count = 0;
      var commonPart = new List<Pair>();

      scanners[0].Position = new Point(0, 0, 0, 0);
      while (scanners.Any(x => x.Position == null))
      {

      }

      for (int i = 0; i < scanners.Count; i++)
      {
        if (scanners[i].Position == null)
        {
          continue;
        }

        for (int j = 0; j < scanners.Count; j++)
        {
          if (i >= j || scanners[j].Position != null)
            continue;

          var scanner1 = scanners[i].GetPairs();
          var scanner2 = scanners[j].GetPairs();
          if (i < j)
          {
            var commonPairs = scanner1.Where(x => scanner2.Any(y => y.Distance == x.Distance)).ToList();
            if (commonPairs.Count() >= requiredMatches)
            {
              var commonPairs2 = scanner2.Where(x => scanner1.Any(y => y.Distance == x.Distance)).ToList();
              if (commonPairs.Count() > requiredMatches)
              {
                //// Clear pairs
                var toRemove = commonPairs.Concat(commonPairs2).SelectMany(x => new[] { x.X, x.Y }).GroupBy(x => x).OrderByDescending(x => x.Count()).Skip(24).Select(x => x.Key);
                commonPairs.RemoveAll(x => toRemove.Any(pointToRemove => pointToRemove == x.X || pointToRemove == x.Y));
                commonPairs2.RemoveAll(x => toRemove.Any(pointToRemove => pointToRemove == x.X || pointToRemove == x.Y));
              }

              var pairsToAdd = new List<Pair>();
              var pairsToAdd2 = new List<Pair>();
              commonPairs.OrderByDescending(x => x.Distance).ForEach(x =>
              {
                if (!pairsToAdd.Any(y => y.ContainsPoint(x.X) || y.ContainsPoint(x.Y)))
                {
                  pairsToAdd.Add(x);
                }
              });

              commonPairs2.OrderByDescending(x => x.Distance).ForEach(x =>
              {
                if (!pairsToAdd2.Any(y => y.ContainsPoint(x.X) || y.ContainsPoint(x.Y)))
                {
                  pairsToAdd2.Add(x);
                }
              });

              var howManyToAdd = 0;
              for (int z = 0; z < pairsToAdd.Count; z++)
              {
                if (!pairsToAdd[z].X.Counted && !pairsToAdd2[z].X.Counted)
                  howManyToAdd++;

                if (!pairsToAdd[z].Y.Counted && !pairsToAdd2[z].Y.Counted)
                  howManyToAdd++;
              }

              var aaa = commonPairs.Concat(commonPairs2).SelectMany(x => new[] { x.X, x.Y }).GroupBy(x => x).OrderByDescending(x => x.Count()).ToList();
              var points = aaa.Select(x => x.Key).Take(numberOfPoints * 2);
              points.ForEach(x => x.Counted = true);
              Console.WriteLine($"Adding {howManyToAdd}");
              count += howManyToAdd;
            }
          }
        }
      }

      Console.WriteLine($"Common points {count}");
      Console.WriteLine($"Unique points {scanners.SelectMany(x => x.Beacons).Count(x => !x.Counted)}");
      Console.WriteLine($"Result {count + scanners.SelectMany(x => x.Beacons).Count(x => !x.Counted)}");
      return count + scanners.SelectMany(x => x.Beacons).Count(x => !x.Counted);
      //scanners.Select(x =>
      //{
      //  for (int i = 0; i < x.Beacons; i++)
      //    for (int j = 0; j < x.Beacons; j++)
      //    {
      //      var p1 = x.Beacons[i];
      //      var p2 = x.Beacons[j];
      //      if (p1 != p2)
      //        pair.Add(Pair.CreatePoint(new Point(i, 0, 0), new Point(j, 0, 0)));
      //    }

      //  x.Beacons.Select(x => )
      //  });

      return pair.Distinct().Count();
    }

    public object? SolvePart2()
    {
      return null;
    }

    private List<Scanner> ParseInput()
    {
      var returnList = new List<Scanner>();
      var currentPointList = new List<Point>();
      var scannerNumber = 0;
      for (int i = 0; i < this.commands.Count(); i++)
      {
        if (string.IsNullOrWhiteSpace(this.commands[i]))
          continue;

        if (this.commands[i].Contains("---"))
        {
          if (currentPointList.Count > 0)
          {
            returnList.Add(new(currentPointList));
            currentPointList = new List<Point>();
            scannerNumber++;
          }

          continue;
        }

        var split = this.commands[i].Split(',').Select(x => int.Parse(x)).ToArray();
        currentPointList.Add(new Point(split[0], split[1], split[2], scannerNumber));
      }

      returnList.Add(new(currentPointList));

      return returnList;
    }
  }
}


