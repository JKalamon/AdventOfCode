namespace AdventOfCode2023;

internal interface IChallenge
{
	string Title { get; }

	string TitleFormat { get; }

	DateTime DateTime { get; }

	string InputPath { get; }

	int Day { get; }

	object SolvePart1();

	object? SolvePart2();
}

internal abstract class ChallengeBase : IChallenge
{
	public abstract string Title { get; }

	public virtual string TitleFormat => $"--- Day {Day}: {Title} ---";

	public virtual DateTime DateTime => new DateTime(2023, 12, Day);

	public virtual string InputPath => $"{Day}{GetType().Name.Replace("Solution", string.Empty)}/input.txt";

	public abstract int Day { get; }

	public abstract object SolvePart1();

	public virtual object? SolvePart2() => null;
}
