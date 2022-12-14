using MoreLinq;

namespace AdventOfCode2022;

internal class HillClimbingAlgorithmSolution : IChallenge
{
	public string Title => "--- Day 12: Hill Climbing Algorithm ---";

	public DateTime DateTime => new(2022, 12, 12);

	private string[] Input = File.ReadAllLines("12HillClimbingAlgorithm/input.txt");

	public object SolvePart1()
	{
		var list = ParseInput();
		Dijkstra(list);

		return list.First(x => x.IsDestination).Distance;
	}

	public object? SolvePart2()
	{
		var list = ParseInput();
		list.ForEach(x =>
		{
			x.Distance = int.MaxValue;
			x.Processed = false;
		});

		list.First(x => x.IsDestination).Distance = 0;
		Dijkstra(list, true);
		return list.Where(x => x.Height == 0).OrderBy(x => x.Distance).First().Distance;
	}

	void Dijkstra(IEnumerable<HillRecord> records, bool reverse = false)
	{
		while (records.Any(x => !x.Processed && x.Distance < int.MaxValue))
		{
			var recordToProcess = records.Where(x => !x.Processed).OrderBy(x => x.Distance).First();
			foreach (var sib in GetSiblings(records, recordToProcess.X, recordToProcess.Y))
			{
				var canGo = reverse ? CanGo(sib, recordToProcess) : CanGo(recordToProcess, sib);
				if (canGo && sib.Distance >= recordToProcess.Distance + 1)
				{
					sib.Distance = recordToProcess.Distance + 1;
				}
			}

			recordToProcess.Processed = true;
		}
	}

	private bool CanGo(HillRecord from, HillRecord to)
	{
		return from.Height + 1 >= to.Height;
	}

	private IEnumerable<HillRecord> GetSiblings(IEnumerable<HillRecord> records, int x, int y)
	{
		return records.Where(hr => (hr.Y == y && (hr.X == x - 1 || hr.X == x + 1) || (hr.X == x && (hr.Y == y - 1 || hr.Y == y + 1))));
	}

	private IEnumerable<HillRecord> ParseInput()
	{
		var list = new List<HillRecord>();
		for (int i = 0; i < Input.Length; i++)
		{
			for (int j = 0; j < Input[i].Length; j++)
			{
				var aa = new HillRecord(Input[i][j], j, i);
				list.Add(aa);
				if (Input[i][j] == 'S')
				{
					aa.Distance = 0;
				}
			}
		}

		return list;
	}

	class HillRecord
	{
		private string priorityList = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

		public HillRecord(int Height, int x, int y)
		{
			this.Height = Height;
			this.X = x;
			this.Y = y;
		}

		public HillRecord(char c, int x, int y)
		{
			if (c == 'S')
			{
				c = 'a';
			}

			if (c == 'E')
			{
				this.IsDestination = true;
				c = 'z';
			}

			this.Height = priorityList.IndexOf(c);
			this.X = x;
			this.Y = y;
		}

		public int Height { get; init; }

		public int X { get; init; }

		public int Y { get; init; }

		public bool IsDestination { get; init; }

		public int Distance { get; set; } = int.MaxValue;

		public bool Processed { get; set; } = false;
	}
}
