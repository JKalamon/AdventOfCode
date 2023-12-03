using System.Diagnostics;
using TextCopy;

namespace AdventOfCode2023;

internal static class ChallengeRunner
{
	public static void RunChallenge(IChallenge challenge)
	{
		ConsoleManager.SetTitle(challenge.TitleFormat, challenge.DateTime);
		ConsoleManager.DrawImage(File.ReadAllLines($"Assets/ChristmasTree{new Random().Next(1, 3)}.txt"));

		GC.Collect();
		var stopwatchPart1 = Stopwatch.StartNew();
		var part1 = challenge.SolvePart1().ToString() ?? "";
		stopwatchPart1.Stop();
		var memoryPart1 = GC.GetTotalMemory(false);

		ClipboardService.SetText(part1);
		ConsoleManager.WriteResult(part1, 1, stopwatchPart1.Elapsed, memoryPart1) ;

		GC.Collect();
		var stopwatchPart2 = Stopwatch.StartNew();
		var part2 = challenge.SolvePart2()?.ToString() ?? "";
		stopwatchPart2.Stop();
		var memoryPart2 = GC.GetTotalMemory(false);
		if (!string.IsNullOrWhiteSpace(part2))
		{
			ClipboardService.SetText(part2);
			ConsoleManager.WriteResult(part2, 2, stopwatchPart2.Elapsed, memoryPart2);
		}

		Console.SetCursorPosition(0, Console.WindowHeight - 5);
	}
}

