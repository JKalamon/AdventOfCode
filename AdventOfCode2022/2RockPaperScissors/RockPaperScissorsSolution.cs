namespace AdventOfCode2022;

internal enum ToolType
{
	Rock = 1,
	Paper = 2,
	Scissors = 3
}

internal static class RockPaperHelper
{
	public static ToolType Beats(this ToolType myTool)
		=> myTool switch
		{
			ToolType.Paper => ToolType.Rock,
			ToolType.Rock => ToolType.Scissors,
			ToolType.Scissors => ToolType.Paper,
			_ => throw new ArgumentException(),
		};

	public static ToolType Looses(this ToolType myTool)
	=> myTool switch
	{
		ToolType.Paper => ToolType.Scissors,
		ToolType.Rock => ToolType.Paper,
		ToolType.Scissors => ToolType.Rock,
		_ => throw new ArgumentException(),
	};

	public static ToolType Parse(string toolString)
		=> toolString switch
		{
			"X" => ToolType.Rock,
			"Y" => ToolType.Paper,
			"Z" => ToolType.Scissors,
			"A" => ToolType.Rock,
			"B" => ToolType.Paper,
			"C" => ToolType.Scissors,
			_ => throw new ArgumentException(),
		};
}

internal record Game(ToolType Opponent, ToolType Me)
{
	bool IsWin => Me.Beats() == Opponent;

	bool IsDraw => Me == Opponent;

	public int Points()
	{
		var points = (int)Me;
		if (this.IsWin)
			points += 6;

		if (this.IsDraw)
			points += 3;

		return points;
	}
}

internal class RockPaperScissorsSolution : IChallenge
{
	public string Title => "--- Day 2: Rock Paper Scissors ---";

	public DateTime DateTime => new(2022, 12, 1);


	public object SolvePart1()
	{
		var list = new List<Game>();
		foreach (var input in File.ReadAllLines("2RockPaperScissors/input.txt"))
		{
			list.Add(new Game(RockPaperHelper.Parse(input.Split(' ')[0]), RockPaperHelper.Parse(input.Split(' ')[1])));
		}

		return list.Select(x => x.Points()).Sum();
	}

	public object? SolvePart2()
	{
		var list = new List<Game>();
		foreach (var input in File.ReadAllLines("2RockPaperScissors/input.txt"))
		{
			var opponent = RockPaperHelper.Parse(input.Split(' ')[0]);
			var me = input.Split(' ')[1] switch
			{
				"X" => opponent.Beats(),
				"Y" => opponent,
				"Z" => opponent.Looses(),
				_ => throw new ArgumentException(),
			};

			list.Add(new Game(opponent, me));
		}

		return list.Select(x => x.Points()).Sum();
	}
}
