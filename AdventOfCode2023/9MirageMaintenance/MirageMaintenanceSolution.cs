using MoreLinq;

namespace AdventOfCode2023;

internal class MirageMaintenanceSolution : ChallengeBase
{
	public override string Title => "Mirage Maintenance";

	public override int Day => 9;

	public override object SolvePart1() => Extrapolate(true);

	public override object SolvePart2() => Extrapolate(false);


	private long Extrapolate(bool nextValue)
	{
		var seq = File.ReadAllLines(this.InputPath).Select(x => x.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray()).ToArray();
		var extraPolationValues = new List<long>();
		seq.ForEach(x =>
		{
			var list = new List<long[]>();
			var currentList = x;
			list.Add(currentList);
			while (!currentList.All(x => x == 0))
			{
				var newList = new long[currentList.Length - 1];
				list.Add(newList);

				for (var i = 1; i < currentList.Length; i++)
				{
					newList[i - 1] = currentList[i] - currentList[i - 1];
				}

				currentList = newList;
			}

			long extrapolateValue = 0;
			for (var i = list.Count() - 2; i >= 0; i--)
			{
				if (nextValue)
				{
					extrapolateValue = list[i].Last() + extrapolateValue;
				}
				else
				{
					extrapolateValue = list[i].First() - extrapolateValue;
				}
			}

			extraPolationValues.Add(extrapolateValue);
		});

		return extraPolationValues.Sum();
	}
}
