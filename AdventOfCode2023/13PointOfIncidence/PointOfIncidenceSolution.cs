using MoreLinq;
using System.Text;

namespace AdventOfCode2023;

internal class PointOfIncidenceSolution : ChallengeBase
{
	public override string Title => "Point of Incidence";

	public override int Day => 13;

	public override object SolvePart1()
	{
		var counter = 0;
		foreach (var map in ParseInput())
		{
			var symetricFound = false;
			for (int i = 1; i < map.Width; i++)
			{
				var howFarFromEdge = Math.Min(i, map.Width - i);

				symetricFound = Enumerable.Range(0, map.Height).All(lineNum =>
				{
					var firstLine = map.GetHorizontalLine(lineNum);
					var segmentToAnalyze = new ArraySegment<bool>(firstLine, i - howFarFromEdge, howFarFromEdge * 2);
					return segmentToAnalyze.SequenceEqual(segmentToAnalyze.Reverse());
				});

				if (symetricFound)
				{
					counter += i;
					break;
				}
			}

			if (symetricFound)
				continue;

			for (int i = 1; i < map.Height; i++)
			{
				var howFarFromEdge = Math.Min(i, map.Height - i);

				symetricFound = Enumerable.Range(0, map.Width).All(colNum =>
				{
					var firstLine = map.GetVerticalLine(colNum);
					var segmentToAnalyze = new ArraySegment<bool>(firstLine, i - howFarFromEdge, howFarFromEdge * 2);
					return segmentToAnalyze.SequenceEqual(segmentToAnalyze.Reverse());
				});

				if (symetricFound)
				{
					counter += i * 100;
					break;
				}
			}
		}

		return counter;
	}

	public override object SolvePart2()
	{
		var counter = 0;
		foreach (var map in ParseInput())
		{
			var symetricFound = false;
			for (int i = 1; i < map.Width; i++)
			{
				var howFarFromEdge = Math.Min(i, map.Width - i);
				symetricFound = Enumerable.Range(0, map.Height).Sum(lineNum =>
				{
					var line = map.GetHorizontalLine(lineNum);
					var segmentToAnalyze = new ArraySegment<bool>(line, i - howFarFromEdge, howFarFromEdge * 2);
					return segmentToAnalyze.NumberOfDifferences(segmentToAnalyze.Reverse());
				}) == 2;


				if (symetricFound)
				{
					counter += i;
					break;
				}
			}

			if (symetricFound)
				continue;

			for (int i = 1; i < map.Height; i++)
			{
				var howFarFromEdge = Math.Min(i, map.Height - i);

				symetricFound = Enumerable.Range(0, map.Width).Sum(colNum =>
				{
					var firstLine = map.GetVerticalLine(colNum);
					var segmentToAnalyze = new ArraySegment<bool>(firstLine, i - howFarFromEdge, howFarFromEdge * 2);
					return segmentToAnalyze.NumberOfDifferences(segmentToAnalyze.Reverse());
				}) == 2;

				if (symetricFound)
				{
					counter += i * 100;
					break;
				}
			}
		}

		return counter;
	}

	private IEnumerable<Map> ParseInput()
	{
		var maps = new List<Map>();
		var y = 0;
		var currentRocks = new List<Rock>();
		File.ReadAllLines(this.InputPath).ForEach(x =>
		{
			if (string.IsNullOrWhiteSpace(x))
			{
				maps.Add(new Map(currentRocks.ToArray()));
				y = 0;
				currentRocks.Clear();
				return;
			}

			x.AllIndexOf(c => c == '#').ForEach(i => currentRocks.Add(new Rock(i, y, true)));
			y++;
		});

		maps.Add(new Map(currentRocks.ToArray()));
		return maps;
	}

	private record Rock(int X, int Y, bool IsRock);

	private record Map(Rock[] Rocks)
	{
		public int Width = Rocks.Max(x => x.X) + 1;

		public int Height = Rocks.Max(x => x.Y) + 1;

		public bool[] GetHorizontalLine(int y)
			=> Enumerable.Range(0, Width).Select(x => this.Rocks.Any(r => r.X == x && r.Y == y)).ToArray();

		public bool[] GetVerticalLine(int x)
			=> Enumerable.Range(0, Height).Select(i => this.Rocks.Any(r => r.Y == i && r.X == x)).ToArray();
	}
}
