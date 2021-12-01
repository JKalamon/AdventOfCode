namespace AdventOfCode2021;

internal static class ChallengeRunner
{
  public static void RunChallenge(IChallenge challenge)
  {
    ConsoleManager.SetTitle(challenge.Title, challenge.DateTime);
    ConsoleManager.WriteResult(challenge.SolvePart1());
    ConsoleManager.WriteResult(challenge.SolvePart2(), 2);
    ConsoleManager.DrawImage(File.ReadAllLines($"Assets/ChristmasTree{new Random().Next(1,4)}.txt"));
  }
}
