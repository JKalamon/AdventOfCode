using MoreLinq;
using System;

namespace AdventOfCode2023;

internal class IfYouGiveASeedAFertilizerSolution : ChallengeBase
{
	public override string Title => "If You Give A Seed A Fertilizer";

	public override int Day => 5;

	public override object SolvePart1()
	{
		var almac = Almanac.ParseInput(File.ReadAllLines(this.InputPath));
		long lowest = long.MaxValue;

		almac.Seeds.ForEach(seed =>
		{
			long current = seed;
			almac.Maps.ForEach(map => current = map.Map(current));
			if (current < lowest) lowest = current;
		});

		return lowest;
	}

	public override object? SolvePart2()
	{
		var almanc = Almanac.ParseInput(File.ReadAllLines(this.InputPath));

		var seeds = almanc.SeedsRange();
		var range = seeds[0];
		long iterator = long.MaxValue;

		(long lowest, long lowestIterator) FindLowest(Almanac almanc, Range seeds, long iteratorStep, long? startStep = null, long maxIterator = long.MaxValue)
		{
			long lowest = long.MaxValue;
			long lowestIterator = long.MaxValue;
			iterator = startStep ?? seeds.Start;
			var current = seeds.Start;
			while (seeds.IsWithinRange(iterator) && iterator < maxIterator)
			{
				current = iterator;
				almanc.Maps.ForEach(map => current = map.Map(current));
				if (current < lowest)
				{
					iterator++;
					lowestIterator = iterator;
					lowest = current;
				}
				else
				{
					iterator += iteratorStep;
				}
			}

			return (lowest, lowestIterator);
		}

		long skipStep = 5000;
		var filter = seeds.ToDictionary(x => x, x => FindLowest(almanc, x, skipStep)).OrderBy(x => x.Value.lowest).First();
		return FindLowest(almanc, filter.Key, 1, filter.Value.lowestIterator - skipStep, filter.Value.lowestIterator + skipStep).lowest;
	}

	public record Range(long Start, long Count)
	{
		public bool IsWithinRange(long number)
			=> number >= Start && number < Start + Count;

		public long GetIndex(long number)
		{
			if (!IsWithinRange(number))
				return -1;

			return number - Start;
		}

		public long GetNumberAtIndex(long index)
		{
			if (index < 0 || index >= Count)
				throw new ArgumentOutOfRangeException("Index is out of range.");

			return Start + index;
		}
	}

	record AlmanacMapRange(Range Destination, Range Source)
	{
		public bool HasMatch(long number) => Source.IsWithinRange(number);

		public long Map(long number) => Destination.GetNumberAtIndex(Source.GetIndex(number));
	}

	record class AlmanacMap(string Name)
	{
		public List<AlmanacMapRange> Ranges = new List<AlmanacMapRange>();

		public long Map(long number)
		{
			var map = Ranges.FirstOrDefault(x => x.HasMatch(number));
			if (map == null)
				return number;
			return map.Map(number);
		}
	}

	record Almanac(long[] Seeds, AlmanacMap[] Maps)
	{
		public Range[] SeedsRange()
		{
			var output = new Range[Seeds.Length / 2];

			for (int i = 0; i < Seeds.Length; i += 2)
			{
				output[i / 2] = new Range(Seeds[i], Seeds[i + 1]);
			}

			return output;
		}


		public static Almanac ParseInput(string[] lines)
		{
			var seeds = lines.First().Replace("seeds: ", string.Empty).Split(' ').Select(long.Parse);

			var maps = new List<AlmanacMap>();
			AlmanacMap? currentMap = null;
			for (int i = 2; i < lines.Count(); i++)
			{
				var line = lines[i];
				if (string.IsNullOrWhiteSpace(line) && currentMap != null)
				{
					currentMap = null;
					continue;
				}

				if (currentMap == null)
				{
					currentMap = new AlmanacMap(line.Split(' ')[0]);
					maps.Add(currentMap);
				}
				else
				{
					var rangeNumbers = line.Split(' ').Select(long.Parse).ToArray();
					currentMap.Ranges.Add(new(new Range(rangeNumbers[0], rangeNumbers[2]), new Range(rangeNumbers[1], rangeNumbers[2])));
				}
			}

			var aa = seeds.ToArray();
			var ma = maps.ToArray();
			return new Almanac(aa, ma);
		}

	}
}
