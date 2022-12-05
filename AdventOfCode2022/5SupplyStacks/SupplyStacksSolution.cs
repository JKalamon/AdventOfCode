using MoreLinq;

namespace AdventOfCode2022;

internal class SupplyStacksSolution : IChallenge
{
	public string Title => "--- Day 5: Supply Stacks ---";

	public DateTime DateTime => new(2022, 12, 5);

	public List<Stack<char>> Stacks = new List<Stack<char>>();

	public object SolvePart1()
	{
		ParseInput();
		var file = File.ReadAllLines("5SupplyStacks/input.txt");
		var index = file.Index().First(x => string.IsNullOrEmpty(x.Value)).Key;

		for (int i = index + 1; i < file.Length; i++)
		{
			var instruction = file[i].Replace("move ", "").Replace("from ", "").Replace("to ", "").Split(" ").Select(x => int.Parse(x)).ToArray();
			for (int j = 0; j < instruction[0]; j++)
			{
				var aa = this.Stacks[instruction[1] - 1].Pop();
				this.Stacks[instruction[2] - 1].Push(aa);
			}
		}

		var result = "";
		for (int i = 0; i < this.Stacks.Count; i++)
		{
			result += this.Stacks[i].Pop();
		}

		return result;
	}

	public object? SolvePart2()
	{
		ParseInput();
		var file = File.ReadAllLines("5SupplyStacks/input.txt");
		var index = file.Index().First(x => string.IsNullOrEmpty(x.Value)).Key;

		for (int i = index + 1; i < file.Length; i++)
		{
			string cratesToMove = "";
			var instruction = file[i].Replace("move ", "").Replace("from ", "").Replace("to ", "").Split(" ").Select(x => int.Parse(x)).ToArray();
			for (int j = 0; j < instruction[0]; j++)
			{
				cratesToMove += this.Stacks[instruction[1] - 1].Pop();
			}

			for (int j = 0; j < instruction[0]; j++)
			{
				this.Stacks[instruction[2] - 1].Push(cratesToMove[instruction[0] - j - 1]);
			}
		}

		var result = "";
		for (int i = 0; i < this.Stacks.Count; i++)
		{
			result += this.Stacks[i].Pop();
		}

		return result;
	}

	private void ParseInput()
	{
		this.Stacks = new List<Stack<char>>();
		var file = File.ReadAllLines("5SupplyStacks/input.txt");
		var index = file.Index().First(x => string.IsNullOrEmpty(x.Value)).Key;
		var rows = int.Parse(file[index - 1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Last());
		for (int i = 0; i < rows; i++)
			this.Stacks.Add(new Stack<char>());

		for (int i = index - 2; i >= 0; i--)
		{
			for (int row = 0; row < rows; row++)
			{
				if (file[i][row * 4 + 1] != ' ')
				{
					this.Stacks[row].Push(file[i][row * 4 + 1]);
				}
			}
		}
	}
}

