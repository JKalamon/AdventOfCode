using MoreLinq;

namespace AdventOfCode2023;

internal class CosmicExpansionSolution : ChallengeBase
{
	public override string Title => "Cosmic Expansion";

	public override int Day => 11;

	public override object SolvePart1()
	{
		var map = ParseMap(File.ReadAllLines(this.InputPath), 1);
		long sumOfDistances = 0;
		
		for (int i = 0; i < map.AllGalaxies.Length; i++)
		{
			var galaxy = map.AllGalaxies[i];
			for (int y = i; y < map.AllGalaxies.Length; y++)
			{
				sumOfDistances += galaxy.Distance(map.AllGalaxies[y]);
			}
		}

		return sumOfDistances;
	}

	public override object SolvePart2()
	{
		//// 1 milion twice as big is 999999 columns or cells
		var map = ParseMap(File.ReadAllLines(this.InputPath), 999999);
		long sumOfDistances = 0;

		for (int i = 0; i < map.AllGalaxies.Length; i++)
		{
			var galaxy = map.AllGalaxies[i];
			for (int y = i; y < map.AllGalaxies.Length; y++)
			{
				sumOfDistances += galaxy.Distance(map.AllGalaxies[y]);
			}
		}

		return sumOfDistances;
	}

	public record Pair(long X, long Y)
	{
		public bool IsTheSame(Pair other) => (X == other.X && Y == other.Y) || (Y == other.X && X == other.Y);
	}

	public record SpaceCell(bool IsGalaxy, long X, long Y)
	{
		public long X { get; set; } = X;

		public long Y { get; set; } = Y;

		public long Distance(SpaceCell other) => Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
	}

	public record GalaxyMap(IEnumerable<SpaceCell> GalaxyCells)
	{
		public SpaceCell Get(int x, int y) => GalaxyCells.FirstOrDefault(c => c.X == x && c.Y == y) ?? new SpaceCell(false, x, y);

		public SpaceCell[] AllGalaxies => GalaxyCells.ToArray();
	}

	public static GalaxyMap ParseMap(string[] lines, long howManyToAdd)
	{
		var galaxyCells = new List<SpaceCell>();
		for (var y = 0; y < lines.Length; y++)
			for (var x = 0; x < lines[y].Length; x++)
				if (lines[y][x] == '#')
					galaxyCells.Add(new SpaceCell(true, x, y));

		//// itereate rows
		Enumerable.Range(0, lines.Length)
			.Where(y => !galaxyCells.Any(x => x.Y == y))
			.OrderByDescending(x => x)
			.ForEach(y => galaxyCells.Where(x => x.Y > y).ForEach(y => y.Y += howManyToAdd));

		//// itereate columns
		Enumerable.Range(0, galaxyCells.Max(x => (int)x.X))
			.Where(x => !galaxyCells.Any(c => c.X == x))
			.OrderByDescending(x => x)
			.ForEach(x => galaxyCells.Where(c => c.X > x).ForEach(c => c.X += howManyToAdd));

		return new GalaxyMap(galaxyCells);
	}
}
