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

	public object? SolvePart2()
	{
		var score = 0;
		for (int y = 0; y < Trees.Length; y++)
		{
			for (int x = 0; x < Trees[0].Length; x++)
			{
				if (IsVisible(x, y) && Score(x, y) > score)
				{
					score = Score(x, y);
				}
			}
		}

		return score;
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
		for (int z = y + 1; z < Trees.Length; z++)
		{
			bottom = bottom && current > Trees[z][x];
		}

		return left || right || bottom || top;
	}

	private int Score(int x, int y)
	{
		var current = Trees[y][x];
		var left = 0;
		for (int z = x - 1; z >= 0; z--)
		{
			if (current <= Trees[y][z])
			{
				left++;
				break;
			}

			left++;
		}

		var right = 0;
		for (int z = x + 1; z < Trees[y].Length; z++)
		{
			if (current <= Trees[y][z])
			{
				right++;
				break;
			}

			right++;
		}

		var top = 0;
		for (int z = y - 1; z >= 0; z--)
		{
			if (current <= Trees[z][x])
			{
				top++;
				break;
			}

			top++;
		}

		var bottom = 0;
		for (int z = y + 1; z < Trees.Length; z++)
		{
			if (current <= Trees[z][x])
			{
				bottom++;
				break;
			}
			bottom++;
		}

		return left * right * bottom * top;
	}
}
