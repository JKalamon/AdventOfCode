using MoreLinq;
using System.Text;

namespace AdventOfCode2023;

internal class LensLibrarySolution : ChallengeBase
{
	public override string Title => "Lens Library";

	public override int Day => 15;

	public override object SolvePart1()
		=> File.ReadAllText(this.InputPath).Split(',', StringSplitOptions.RemoveEmptyEntries).Sum(Hash);

	public record BoxLens(string Label, int Value)
	{
		public int Value { get; set; } = Value;
	}

	public override object SolvePart2()
	{
		var mainDic = new Dictionary<int, List<BoxLens>>();
		var EnsureDic = (int aa) =>
		{
			if (mainDic.ContainsKey(aa))
				return mainDic[aa];

			var dic = new List<BoxLens>();
			mainDic.Add(aa, dic);
			return dic;
		};

		ParseInput().ForEach(x =>
		{
			var dic = EnsureDic(x.Hash);
			var exsistingRow = dic.FirstOrDefault(y => x.Label == y.Label);
			if (x.SetValue.HasValue)
			{
				if (exsistingRow != null)
					exsistingRow.Value = x.SetValue.Value;
				else
					dic.Add(new BoxLens(x.Label, x.SetValue.Value));
			}
			else
			{
				if (exsistingRow != null)
					dic.Remove(exsistingRow);
			}
		});

		var value = 0;
		mainDic.Keys.ForEach(boxKey =>
		{
			mainDic[boxKey].ForEach((x, i) =>
			{
				value += (boxKey + 1) * (i + 1) * x.Value;
			});
		});

		return value;
	}

	private static int Hash(string value)
		=> Encoding.ASCII.GetBytes(value).Aggregate(0, (acc, x) => ((acc + x) * 17) % 256);

	private record BoxRecord(string Label, int? SetValue)
	{
		public int Hash => Hash(Label);
	}

	private IEnumerable<BoxRecord> ParseInput()
		=> File.ReadAllText(this.InputPath).Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x =>
		{
			if (x.EndsWith('-'))
				return new BoxRecord(x[..^1], null);

			return new BoxRecord(x.Split('=')[0], int.Parse(x.Split('=')[1]));
		});
}
