namespace AdventOfCode2022;

internal class CalorieCoutingSolution : IChallenge
{
	public string Title => "--- Day 1: Calorie Couting ---";

	public DateTime DateTime => new(2022, 12, 1);


	public object SolvePart1()
	{
		return ParseInput().Select(x => x.Sum()).Max();
	}

	public object? SolvePart2()
	{
		return ParseInput().Select(x => x.Sum()).OrderByDescending(x => x).Take(3).Sum();
	}

	private IEnumerable<IEnumerable<int>> ParseInput()
	{
		var list = new List<List<int>>();
		var elf = new List<int>();
		foreach (var input in File.ReadAllLines("1CalorieCounting/input.txt"))
		{
			if (string.IsNullOrEmpty(input))
			{
				list.Add(elf);
				elf = new List<int>();
				continue;
			}

			elf.Add(int.Parse(input));
		}

		if (elf.Count > 0)
		{
			list.Add(elf);
		}

		return list;
	}
}
