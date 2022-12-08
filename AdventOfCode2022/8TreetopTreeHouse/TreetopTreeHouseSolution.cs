namespace AdventOfCode2022;

internal class TreetopTreeHouseSolution : IChallenge
{
	public string Title => "--- Day 8: Treetop Tree House ---";

	public DateTime DateTime => new(2022, 12, 8);

	private int[][] Trees = File.ReadAllLines("8TreetopTreeHouse/input.txt").Select(x => x.ToCharArray()).Select(x => x.Select(y => int.Parse(y.ToString())).ToArray()).ToArray();

	public object SolvePart1()
	{
		var counter = 0;
		for (int y = 0; y < Trees.Length; y++)
		{
			for (int x = 0; x < Trees[0].Length; x++)
			{
				if (IsVisible(x, y))
					counter++;
			}
		}

		return counter;
	}

	private bool IsVisible(int x, int y)
	{
		if (x == 0 || x == Trees[y].Length - 1)
			return true;

		if (y == 0 || y == Trees.Length - 1)
			return true;

		var current = Trees[y][x];
		var left = Trees[y].Take(x).All(z => z < current);
		var right = Trees[y].Skip(x + 1).All(z => z < current);

		var top = true;
		for (int z = y - 1; z >= 0; z--)
		{
			top = top && current > Trees[z][x];
		}

		var bottom = true;
		for (int z = y + 1; z <  Trees.Length; z++)
		{
			bottom = bottom && current > Trees[z][x];
		}

		return left || right || bottom || top;
	}

	public object? SolvePart2()
	{
		return null;
	}
}
