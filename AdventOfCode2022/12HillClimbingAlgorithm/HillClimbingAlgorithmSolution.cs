namespace AdventOfCode2022;

internal class HillClimbingAlgorithmSolution : IChallenge
{
	public string Title => "--- Day 12: Hill Climbing Algorithm ---";

	public DateTime DateTime => new(2022, 12, 12);

	private string[] Input = File.ReadAllLines("12HillClimbingAlgorithm/input.txt");

	private HillRecord Current = new HillRecord('S', 0, 0);

	public object SolvePart1()
	{
		var list = ParseInput();
		return "ss";
	}

	public object? SolvePart2()
	{
		return null;
	}

	private HillRecord[][] ParseInput()
	{
		var list = new HillRecord[Input.Length][];
		for (int i = 0; i < Input.Length; i++)
		{
			list[i] = new HillRecord[Input[i].Length];
			for (int j = 0; j < Input[i].Length; j++)
			{
				list[i][j] = new HillRecord(Input[i][j], j, i);
				if (list[i][j].IsCurrent)
				{
					this.Current = list[i][j];
				}
			}
		}

		return list;
	}

	class HillRecord
	{
		private string priorityList = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

		public HillRecord(char c, int x, int y)
		{
			if (c == 'S')
			{
				this.IsCurrent = true;
				c = 'a';
			}

			if (c == 'E')
			{
				this.IsDestination = true;
				c = 'z';
			}

			this.Height = priorityList.IndexOf(c);
			this.X = x;
			this.Y = y;
		}

		public int Height { get; init; }

		public int X { get; init; }

		public int Y { get; init; }

		public bool IsCurrent { get; set; }

		public bool IsDestination { get; init; }
	}
}
