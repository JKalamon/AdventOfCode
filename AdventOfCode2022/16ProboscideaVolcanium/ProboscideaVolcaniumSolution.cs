using MoreLinq;
using System.Text.RegularExpressions;

namespace AdventOfCode2022;

internal class ProboscideaVolcaniumSolution : IChallenge
{
	public string Title => "--- Day 16: Proboscidea Volcanium ---";

	public DateTime DateTime => new(2022, 12, 15);

	public IList<PumpPoint> AllPoints = new List<PumpPoint>();

	private IEnumerable<string> Input = File.ReadAllLines("16ProboscideaVolcanium/input.txt");
	public object SolvePart1()
	{
		ParseInput();
		AllPoints.ForEach(Dijkstra);
		var runningPipes = new List<PumpPoint>();
		var currentMinute = 0;
		var minuteLimit = 30;
		var currentPoint = AllPoints.First(x => x.Name == "AA");
		var value = 0;
		
		return Resolve(0, minuteLimit, 0, currentPoint, Array.Empty<PumpPoint>(), 5, new Log(Array.Empty<string>(), 0)).val;
	}

	public object? SolvePart2()
	{
		ParseInput();
		AllPoints.ForEach(Dijkstra);
		var runningPipes = new List<PumpPoint>();
		var currentMinute = 0;
		var minuteLimit = 30;
		var currentPoint = AllPoints.First(x => x.Name == "AA");
		var value = 0;

		return Resolve2(0, minuteLimit, 0, currentPoint, Array.Empty<PumpPoint>(), 5, new Log(Array.Empty<string>(), 0)).val;
	}

	public Log Resolve(int currentMinute, int maxMinutes, int value, PumpPoint currentPoint, IEnumerable<PumpPoint> runningPipes, int howManyTake, Log log)
	{
		var timeLeft = maxMinutes - currentMinute;
		var options = currentPoint.Distances.Where(x => x.Dist < (timeLeft - 2) && !runningPipes.Contains(x.Pump) && x.Pump.FlowRate > 0).OrderByDescending(x => x.Efficiency).Take(howManyTake);

		if (!options.Any())
		{
			while (currentMinute < maxMinutes)
			{
				runningPipes.ForEach(x => value += x.FlowRate);
				currentMinute++;
			}

			return new Log(MoreLinq.Extensions.AppendExtension.Append(log.x, "wait to 30 minutes"), value);
		}
		else
		{
			var aaa = options.Select(goTo =>
			{
				var tmpVal = value;
				runningPipes.ForEach(x => tmpVal += x.FlowRate * (goTo.Dist + 1));
				return Resolve(currentMinute + goTo.Dist + 1, maxMinutes, tmpVal, goTo.Pump, MoreLinq.Extensions.AppendExtension.Append(runningPipes, goTo.Pump), howManyTake,
					new Log(MoreLinq.Extensions.AppendExtension.Append(log.x, $"chosen path {goTo.Pump.Name} it took {goTo.Dist} + 1"), value));
			});

			if (currentMinute == 0)
			{
				aaa.OrderByDescending(x => x.val).First().x.ForEach(x =>
				{
					Console.WriteLine(x);
				});
			}

			return aaa.OrderByDescending(x => x.val).First();
		}
	}

	public Log Resolve2(int currentMinute, int maxMinutes, int value, PumpPoint[] currentPoint, IEnumerable<PumpPoint> runningPipes, int howManyTake, Log log)
	{
		var timeLeft = maxMinutes - currentMinute;
		var options = currentPoint[0].Distances.Where(x => x.Dist < (timeLeft - 2) && !runningPipes.Contains(x.Pump) && x.Pump.FlowRate > 0).OrderByDescending(x => x.Efficiency).Take(howManyTake);
		var options2 = currentPoint[1].Distances.Where(x => x.Dist < (timeLeft - 2) && !runningPipes.Contains(x.Pump) && x.Pump.FlowRate > 0).OrderByDescending(x => x.Efficiency).Take(howManyTake);

		if (!options.Any())
		{
			while (currentMinute < maxMinutes)
			{
				runningPipes.ForEach(x => value += x.FlowRate);
				currentMinute++;
			}

			return new Log(MoreLinq.Extensions.AppendExtension.Append(log.x, "wait to 30 minutes"), value);
		}
		else
		{
			var aaa = options.Select(goTo =>
			{
				var tmpVal = value;
				runningPipes.ForEach(x => tmpVal += x.FlowRate * (goTo.Dist + 1));
				return Resolve(currentMinute + goTo.Dist + 1, maxMinutes, tmpVal, goTo.Pump, MoreLinq.Extensions.AppendExtension.Append(runningPipes, goTo.Pump), howManyTake,
					new Log(MoreLinq.Extensions.AppendExtension.Append(log.x, $"chosen path {goTo.Pump.Name} it took {goTo.Dist} + 1"), value));
			});

			if (currentMinute == 0)
			{
				aaa.OrderByDescending(x => x.val).First().x.ForEach(x =>
				{
					Console.WriteLine(x);
				});
			}

			return aaa.OrderByDescending(x => x.val).First();
		}
	}

	void Dijkstra(PumpPoint startPoint)
	{
		AllPoints.ForEach(x =>
		{
			x.Processed = false;
			x.Distance = int.MaxValue;
		});

		startPoint.Distance = 0;
		while (AllPoints.Any(x => !x.Processed && x.Distance < int.MaxValue))
		{
			var recordToProcess = AllPoints.Where(x => !x.Processed).OrderBy(x => x.Distance).First();
			foreach (var sib in recordToProcess.Paths)
			{
				if (sib.Distance >= recordToProcess.Distance + 1)
					sib.Distance = recordToProcess.Distance + 1;
			}

			recordToProcess.Processed = true;
		}

		AllPoints.ForEach(x =>
		{
			startPoint.Distances.Add(new Distance(x.Distance, x));
		});
	}

	public void ParseInput()
	{
		this.AllPoints = Input.Select(x => new PumpPoint(x.Substring(6, 2), int.Parse(x.Substring(23, 2).Replace(";", "")))).ToList();
		Input.ForEach(x =>
		{
			var groups = Regex.Match(x, @"Valve ([A-Z]{2}) has flow rate=\d{1,2}; tunnels{0,1} leads{0,1} to valves{0,1} (.+)").Groups;
			var groupToAddPaths = this.AllPoints.First(x => x.Name == groups[1].Value);
			var paths = groups[2].Value.Split(", ");

			paths.ForEach(x =>
			{
				groupToAddPaths.Paths.Add(this.AllPoints.First(y => y.Name == x));
			});
		});
	}

	public record Distance(int Dist, PumpPoint Pump)
	{
		public double Efficiency => this.Pump.FlowRate / (this.Dist + 1);
	}

	public record Log(IEnumerable<string> x, int val);

	//public record Log(int minute, , PumpPoint Pump)
	//{
	//	public double Efficiency => this.Pump.FlowRate / (this.Dist + 1);
	//}

	public record PumpPoint(string Name, int FlowRate)
	{
		public IList<PumpPoint> Paths = new List<PumpPoint>();

		public int Distance = int.MaxValue;

		public bool Processed = false;

		public IList<Distance> Distances = new List<Distance>();
	}
}