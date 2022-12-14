using TextCopy;

namespace AdventOfCode2022;

internal static class ChallengeRunner
{
	public static void RunChallenge(IChallenge challenge)
	{
		ConsoleManager.SetTitle(challenge.Title, challenge.DateTime);
		ConsoleManager.DrawImage(File.ReadAllLines($"Assets/ChristmasTree{new Random().Next(1, 3)}.txt"));

		var part1 = challenge.SolvePart1().ToString() ?? "";
		ClipboardService.SetText(part1);
		ConsoleManager.WriteResult(part1);

		var part2 = challenge.SolvePart2()?.ToString() ?? "";
		if (!string.IsNullOrWhiteSpace(part2))
		{
			ClipboardService.SetText(part2);
			ConsoleManager.WriteResult(part2, 2);
		}

		Console.SetCursorPosition(0, Console.WindowHeight - 5);
	}
}

