namespace AdventOfCode2022;

internal class TuningTroubleSolution : IChallenge
{
	public string Title => "--- Day 6: Tuning Trouble ---";

	public DateTime DateTime => new(2022, 12, 6);

	public object SolvePart1()
	{
		var file = File.ReadAllText("6TuningTrouble/input.txt");
		return ParseInput(file, 4);
	}

	public object? SolvePart2()
	{
		var file = File.ReadAllText("6TuningTrouble/input.txt");
		return ParseInput(file, 14);
	}

	private int ParseInput(string input, int length)
	{
		for (int i = length - 1; i < input.Length; i++)
		{
			var col = new List<char>();
			foreach (var j in Enumerable.Range(i - length + 1, length))
			{
				col.Add(input[j]);
			}

			if (col.Distinct().Count() == length)
			{
				return i + 1;
			}
		}

		return 0;
	}
}

