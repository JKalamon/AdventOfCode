namespace AdventOfCode2022;

internal class RucksackReorganizationSolution : IChallenge
{
	public string Title => "--- Day 3: Rucksack Reorganization ---";

	public DateTime DateTime => new(2022, 12, 3);

	public string[] Input = File.ReadAllLines("3RucksackReorganization/input.txt");

	private string priorityList = "_abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

	public object SolvePart1()
	{
		var bothContainsList = new List<char>();
		foreach (var line in Input)
		{
			var split = line.Length / 2;
			var p1 = line.Substring(0, split);
			var p2 = line.Substring(split, line.Length - split);
			for (int i = 0; i < split; i++)
			{
				if (p2.Contains(p1[i]))
				{
					bothContainsList.Add(p1[i]);
					break;
				}
			}
		}

		return CountScore(bothContainsList);
	}

	public object? SolvePart2()
	{
		var bothContainsList = new List<char>();
		for (int i = 0; i < Input.Length; i += 3)
		{
			var firstLine = Input[i];
			for (int j = 0; j < firstLine.Length; j++)
			{
				if (Input[i + 1].Contains(firstLine[j]) && Input[i + 2].Contains(firstLine[j]))
				{
					bothContainsList.Add(firstLine[j]);
					break;
				}
			}
		}

		return CountScore(bothContainsList);
	}

	private int CountScore(IEnumerable<char> input)
	{
		var result = 0;

		foreach (var item in input)
		{
			result += priorityList.IndexOf(item);
		}

		return result;
	}
}
