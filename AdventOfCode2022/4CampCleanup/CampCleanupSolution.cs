namespace AdventOfCode2022;

record Pair(int Start, int End)
{
	public bool Contains(Pair other)
		=> this.Start <= other.Start && this.End >= other.End;

	public bool Overlaps(Pair other)
		=> (this.Start <= other.Start && other.Start <= this.End) || (this.Start <= other.End && other.End <= this.End);
}

internal class CampCleanupSolution : IChallenge
{
	public string Title => "--- Day 4: Camp Cleanup ---";

	public DateTime DateTime => new(2022, 12, 4);

	public IEnumerable<Pair[]> Input = File.ReadAllLines("4CampCleanup/input.txt")
		.Select(x => x.Split(','))
		.Select(x => x.Select(y => new Pair(int.Parse(y.Split("-")[0]), int.Parse(y.Split("-")[1]))).ToArray());

	public object SolvePart1()
		=> Input.Count(x => x[0].Contains(x[1]) || x[1].Contains(x[0]));

	public object? SolvePart2()
		=> this.Input.Count(x => x[0].Overlaps(x[1]) || x[1].Overlaps(x[0]));
}
