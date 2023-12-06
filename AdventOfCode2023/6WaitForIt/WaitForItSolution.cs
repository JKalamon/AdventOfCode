namespace AdventOfCode2023;

internal class WaitForItSolution : ChallengeBase
{
	public override string Title => "Wait For It";

	public override int Day => 6;

	public override object SolvePart1()
	{
		var races = BoatRaces.ParseInput(File.ReadAllLines(this.InputPath));
		var counter = 1;
		for (int i = 0; i < races.Time.Length; i++)
		{
			var time = races.Time[i];
			var maxDistance = races.Records[i];
			counter *= Enumerable.Range(0, time).Where(x => (time - x) * x > maxDistance).Count();
		}

		return counter;
	}

	public override object? SolvePart2()
	{
		var races = BoatRaces.ParseInput(File.ReadAllLines(this.InputPath));
		var time = int.Parse(string.Concat(races.Time.Select(x => x.ToString())));
		var maxDistance = long.Parse(string.Concat(races.Records.Select(x => x.ToString())));
		return Enumerable.Range(0, time).Where(x => ((double)time - x) * x > maxDistance).Count();
	}

	public record BoatRaces(int[] Time, int[] Records)
	{
		public static BoatRaces ParseInput(string[] input)
		{
			var time = input[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
			var records = input[1].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
			return new BoatRaces(time, records);
		}
	}
}
