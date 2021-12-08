using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2021
{
  internal class HydrothermalVentureSolution : IChallenge
  {
    public string Title => "--- Day 5: Hydrothermal Venture ---";

    public DateTime DateTime => new(2021, 12, 5);

    private IEnumerable<string> commands = File.ReadAllLines("5HydrothermalVenture/input.txt");

    record Point(int X, int Y)
    {
      public static Point FromString(string input)
      {
        return new Point(int.Parse(input.Split(',')[0]), int.Parse(input.Split(',')[1]));
      }
    }

    record Line(Point Start, Point End, bool IgnoreDiagonals = true)
    {
      public IEnumerable<Point> GetAffectedPoints()
      {
        if (IgnoreDiagonals && !IsVertical && !IsHorizontal)
          return new Point[0];

        if (IsVertical)
        {
          var xx = Enumerable.Range(Start.Y < End.Y ? Start.Y : End.Y, Math.Abs(End.Y - Start.Y) + 1).Select(y => new Point(Start.X, y));
          return Enumerable.Range(Start.Y < End.Y ? Start.Y : End.Y, Math.Abs(End.Y - Start.Y) + 1).Select(y => new Point(Start.X, y));
        }

        if (IsHorizontal)
        {
          return Enumerable.Range(Start.X < End.X ? Start.X : End.X, Math.Abs(End.X - Start.X) + 1).Select(x => new Point(x, Start.Y));
        }

        var points = new List<Point>();

        var currentPoint = Start;
        points.Add(currentPoint);
        for (var i = 0; i < Math.Abs(Start.X - End.X); i++)
        {
          var xStep = Start.X - End.X < 0 ? 1 : -1;
          var yStep = Start.Y - End.Y < 0 ? 1 : -1;
          currentPoint = new Point(currentPoint.X + xStep, currentPoint.Y + yStep);
          points.Add(currentPoint);

        }

        return points;
      }

      public bool IsVertical => Start.X == End.X;

      public bool IsHorizontal => Start.Y == End.Y;
    }

    public object SolvePart1()
    {
      var inputLines = this.commands.Select(x =>
      {
        var start = Point.FromString(x.Split(" -> ")[0]);
        var end = Point.FromString(x.Split(" -> ")[1]);
        return new Line(start, end);
      });

      return inputLines.SelectMany(x => x.GetAffectedPoints()).GroupBy(x => x).Where(x => x.Count() >= 2).Count().ToString();
    }

    public object? SolvePart2()
    {
      var inputLines = this.commands.Select(x =>
      {
        var start = Point.FromString(x.Split(" -> ")[0]);
        var end = Point.FromString(x.Split(" -> ")[1]);
        return new Line(start, end, false);
      });

      return inputLines.SelectMany(x => x.GetAffectedPoints()).GroupBy(x => x).Where(x => x.Count() >= 2).Count().ToString();
    }
  }
}