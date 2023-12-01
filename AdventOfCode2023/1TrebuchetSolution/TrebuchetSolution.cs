using MoreLinq.Extensions;
using System.Text.RegularExpressions;

namespace AdventOfCode2023;

internal class TrebuchetSolution : IChallenge
{
	public string Title => "Trebuchet?!";

	public int Day => 1;

	public object SolvePart1()
	{
		var numbers = new List<int>();
		foreach (var input in File.ReadAllLines("1TrebuchetSolution/input.txt"))
		{
			var stripped = Regex.Replace(input, "[^0-9]", "");
			numbers.Add(int.Parse([stripped.First(), stripped.Last()]));
		}

		return numbers.Sum();
	}

	public object? SolvePart2()
	{
		var numbers = new List<int>();
		var numbersStringDictionary = new Dictionary<string, int>()
		{
			["one"] = 1,
			["two"] = 2,
			["three"] = 3,
			["four"] = 4,
			["five"] = 5,
			["six"] = 6,
			["seven"] = 7,
			["eight"] = 8,
			["nine"] = 9,
		};

		var text = File.ReadAllText("1TrebuchetSolution/input.txt");
		numbersStringDictionary.ForEach(x => text = text.Replace(x.Key, x.Key.First() + x.Value.ToString() + x.Key.Last()));
		
		foreach (var input in text.Split('\n'))
		{
			
			var stripped = Regex.Replace(input, "[^0-9]", "");
			numbers.Add(int.Parse([stripped.First(), stripped.Last()]));
		}

		return numbers.Sum();
	}
}
