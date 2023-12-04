using MoreLinq;

namespace AdventOfCode2021;

internal class ChitonSolution : IChallenge
{
	public string Title => "--- Day 15: Chiton ---";

	public DateTime DateTime => new(2021, 12, 15);

	private string[] Input = File.ReadAllLines("15Chiton/input.txt");

	public object SolvePart1()
	{
		var list = ParseInput();
		var startPoint = list.First(x => x.X == 0 && x.Y == 0);
		list.First(x => x.Y == Input.Length - 1 && x.X == Input[0].Length - 1).IsDestination = true;
		startPoint.Distance = 0;
		Dijkstra(list);

		return list.First(x => x.IsDestination).Distance;
	}

	public object? SolvePart2()
	{
		var list = ParseInput2();
		var startPoint = list.First(x => x.X == 0 && x.Y == 0);
		list.First(x => x.Y == Input.Length * 5 - 1 && x.X == Input[0].Length * 5 - 1).IsDestination = true;
		startPoint.Distance = 0;

		AStar(list);

		return list.First(x => x.IsDestination).Distance;
	}

	void Dijkstra(IEnumerable<HillRecord> records, bool reverse = false)
	{
		var priorityQueue = new PriorityQueue<HillRecord, int>();
		foreach (var record in records)
		{
			if (!record.Processed)
				priorityQueue.Enqueue(record, record.Distance);
		}

		var destination = records.First(x => x.IsDestination);
		while (priorityQueue.Count > 0)
		{
			Console.WriteLine(priorityQueue.Count);
			var recordToProcess = priorityQueue.Dequeue();
			if (recordToProcess.Distance > destination.Distance && destination.Distance < int.MaxValue)
			{
				recordToProcess.Processed = true;
				continue;
			}

			foreach (var sib in GetSiblings(records, recordToProcess.X, recordToProcess.Y))
			{
				if (sib.Distance > recordToProcess.Distance + sib.Risk)
				{
					sib.Distance = recordToProcess.Distance + sib.Risk;
					priorityQueue.Enqueue(sib, sib.Distance);
				}
			}

			recordToProcess.Processed = true;
		}
	}

	void AStar(IEnumerable<HillRecord> records)
	{
		var openSet = new PriorityQueue<HillRecord, int>();
		var start = records.First(x => x.Distance == 0);
		var goal = records.First(x => x.IsDestination);
		openSet.Enqueue(start, 0);

		while (openSet.Count > 0)
		{
			var current = openSet.Dequeue();

			if (current.X == goal.X && current.Y == goal.Y)
			{
				// Goal reached
				break;
			}

			foreach (var neighbor in GetSiblings(records, current.X, current.Y))
			{
				var tentativeDistance = current.Distance + neighbor.Risk;

				if (tentativeDistance < neighbor.Distance)
				{
					neighbor.Distance = tentativeDistance;
					openSet.Enqueue(neighbor, tentativeDistance + Heuristic(neighbor, goal.X, goal.Y));
				}
			}
		}
	}

	int Heuristic(HillRecord node, int goalX, int goalY)
	{
		// You need to define a heuristic function based on your problem.
		// Common heuristics include Euclidean distance, Manhattan distance, etc.
		// The heuristic should be admissible, i.e., it should never overestimate the cost.
		return Math.Abs(node.X - goalX) + Math.Abs(node.Y - goalY);
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
				var aa = new HillRecord(int.Parse(Input[i][j].ToString()), j, i);
				list.Add(aa);
			}
		}

		return list;
	}

	private IEnumerable<HillRecord> ParseInput2()
	{
		var list = new List<HillRecord>();
		for (int y = 0; y < Input.Length * 5; y++)
		{
			for (int x = 0; x < Input[0].Length * 5; x++)
			{
				var aa = int.Parse(Input[y % Input.Length][x % Input[0].Length].ToString());
				aa += (int)Math.Floor((double)y / Input.Length) + (int)Math.Floor((double)x / Input[0].Length);
				if (aa >= 10)
					aa = (aa + 1) % 10;
				list.Add(new HillRecord(aa, x, y));
			}
		}

		return list;
	}

	class HillRecord
	{
		public HillRecord(int risk, int x, int y)
		{
			this.Risk = risk;
			this.X = x;
			this.Y = y;
		}

		public int Risk { get; init; }

		public int X { get; init; }

		public int Y { get; init; }

		public bool IsDestination { get; set; }

		public int Distance { get; set; } = int.MaxValue;

		public bool Processed { get; set; } = false;
	}
}
