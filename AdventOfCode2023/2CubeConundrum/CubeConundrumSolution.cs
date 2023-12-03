using System.Text.RegularExpressions;

namespace AdventOfCode2023;

internal class CubeConundrumSolution : ChallengeBase
{
	public override string Title => "Cube Conundrum";

	public override int Day => 2;

	public override object SolvePart1()
	{
		var games = ParseInput();
		var redMax = 12;
		var greenMax = 13;
		var blueMax = 14;

		return games.Where(x => x.MaxColorUsed("blue") <= blueMax && x.MaxColorUsed("red") <= redMax && x.MaxColorUsed("green") <= greenMax).Sum(x => x.Id);
	}

	public override object? SolvePart2()
	{
		var games = ParseInput();
		return games.Select(x =>
			x.MaxColorUsed("blue") * x.MaxColorUsed("red") * x.MaxColorUsed("green")).Sum();
	}

	private IEnumerable<Game> ParseInput()
	{
		var retrunVal = new List<Game>();
		foreach (var input in File.ReadAllLines(this.InputPath))
		{
			var gameString = input.Split(":")[0];
			var setsString = input.Split(":")[1];
			var id = int.Parse(Regex.Match(gameString, @"Game ([0-9]+)").Groups[1].Value);

			var cubeSets = setsString.Split(";").Select(a => a.Trim())
				.Select(a =>
					new CubeSet(a.Split(",").Select(b =>
					{
						var match = Regex.Match(b, @"([0-9]+) (blue|red|green)");
						return new Cube(int.Parse(match.Groups[1].Value), match.Groups[2].Value);
					})));

			retrunVal.Add(new Game(id, cubeSets));
		}

		return retrunVal;
	}

	internal record Cube(int Number, string Color);

	internal record CubeSet(IEnumerable<Cube> Set)
	{
		public int ColorUsed(string color) => Set.Where(x => x.Color == color).Sum(x => x.Number);
	}


	internal record Game(int Id, IEnumerable<CubeSet> CubeSet)
	{
		public int MaxColorUsed(string color) => CubeSet.Max(a => a.ColorUsed(color));
	}
}
