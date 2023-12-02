namespace AdventOfCode2023
{
	internal interface IChallenge
	{
		string Title { get; }

		string TitleFormat => $"--- Day {Day}: {Title} ---";

		DateTime DateTime => new(2023, 12, Day);

		int Day { get; }

		object SolvePart1();

		object? SolvePart2();

	}
}

