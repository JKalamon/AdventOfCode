using MoreLinq;
using System.Text.RegularExpressions;

namespace AdventOfCode2022;

internal class MonkeyInTheMiddleSolution : IChallenge
{
	public string Title => "--- Day 11: Monkey in the Middle ---";

	public DateTime DateTime => new(2022, 12, 11);

	private string[] Input = File.ReadAllLines("11MonkeyInTheMiddle/input.txt");

	public object SolvePart1()
	{
		var counter = 0;
		ParseInput();
		return counter;
	}

	public object? SolvePart2()
	{
		return null;
	}

	private IEnumerable<Monkey> ParseInput()
	{
		var list = new List<Monkey>();
		foreach (var lines in Input.Split(string.IsNullOrWhiteSpace))
		{
			var monkeyInput = lines.ToArray();
			var id = Regex.Match(monkeyInput[0], @"Monkey ([0-9]+):").Groups[1].Value;
			var startingItems = Regex.Match(monkeyInput[1], @"Starting items: (.+)").Groups[1].Value.Split(", ").Select(int.Parse);
			var operation = Regex.Match(monkeyInput[2], @"Operation: (.+)").Groups[1].Value;
			var test = int.Parse(Regex.Match(monkeyInput[3], @"Test: divisible by (.+)").Groups[1].Value);
			var trueMonkey = int.Parse(Regex.Match(monkeyInput[4], @"If true: throw to monkey (.+)").Groups[1].Value);
			var falseMonkey = int.Parse(Regex.Match(monkeyInput[5], @"If false: throw to monkey (.+)").Groups[1].Value);
			//new Regex(@"Monkey \d:").Matches(monkeyInput[0]).
		}

		return list;
	}
}

internal record Monkey(int Id, int[] Items, string Operation, int Test, int TrueMonkeyThrow, int FalseMonkeyThrow);
