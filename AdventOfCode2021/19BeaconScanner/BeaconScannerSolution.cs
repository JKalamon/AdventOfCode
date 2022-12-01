using MoreLinq;

namespace AdventOfCode2021
{
	internal class BeaconScannerSolution : IChallenge
	{
		public string Title => "--- Day 19: Beacon Scanner ---";

		public DateTime DateTime => new(2021, 12, 19);

		private string[] commands = File.ReadAllLines("19BeaconScanner/input.txt");


		public record Scanner(List<Point> Beacons)
		{
			public IEnumerable<Pair> GetPairs()
			{
				var pairs = new List<Pair>();
				for (int i = 0; i < this.Beacons.Count; i++)
				{
					for (int j = 0; j < this.Beacons.Count; j++)
					{
						var p1 = this.Beacons[i];
						var p2 = this.Beacons[j];
						var pair = Pair.CreatePoint(p1, p2);
						if (p1 != p2 && !pairs.Contains(pair))
							pairs.Add(pair);
					}
				}

				return pairs;
			}
		}

		public record Pair(Point X, Point Y)
		{
			public static Pair CreatePoint(Point px, Point py)
			{
				if (px.X < py.X)
					return new Pair(px, py);

				if (px.X == py.X && px.Y < py.Y)
					return new Pair(px, py);

				if (px.X == py.X && px.Y == py.Y && px.Z < py.Z)
					return new Pair(px, py);

				return new Pair(py, px);
			}

			public double Distance { get; set; } = X.DistanceFromPoint(Y);
		}


		public record Point(int X, int Y, int Z, int Parent)
		{
			public double DistanceFromPoint(Point b)
			{
				return Math.Pow(this.X - b.X, 2) + Math.Pow(this.Y - b.Y, 2) + Math.Pow(this.Z - b.Z, 2);
			}

			public bool Counted { get; set; } = false;
		}

		public object SolvePart1()
		{
			//// 6 matches
			////var numberOfPoints = 6;
			////var requiredMatches = 15;

			////// 12 matches
			var numberOfPoints = 12;
			var requiredMatches = 66;
			var scanners = ParseInput();
			var pair = new List<Pair>();
			for (int i = 0; i < numberOfPoints; i++)
				for (int j = 0; j < numberOfPoints; j++)
				{
					if (j != i)
						pair.Add(Pair.CreatePoint(new Point(i, 0, 0, 0), new Point(j, 0, 0, 1)));
				}

			var pairsFromScanner1 = scanners[0].GetPairs();
			var pairsFromScanner2 = scanners[1].GetPairs();
			var count = 0;
			var commonPart = new List<Pair>();

			for (int i = 0; i < scanners.Count; i++)
			{
				for (int j = 0; j < scanners.Count; j++)
				{
					var scanner1 = scanners[i].GetPairs();
					var scanner2 = scanners[j].GetPairs();
					if (i < j)
					{
						var commonPairs = scanner1.Where(x => scanner2.Any(y => y.Distance == x.Distance));
						if (commonPairs.Count() >= requiredMatches)
						{
							if (commonPairs.Count() > requiredMatches)
							{
								Console.WriteLine("test");
							}
							var commonPairs2 = scanner2.Where(x => scanner1.Any(y => y.Distance == x.Distance));

							Console.WriteLine($"{i} - {j}, common lines: " + commonPairs.Count());
							var aaa = commonPairs.Concat(commonPairs2).SelectMany(x => new[] { x.X, x.Y }).GroupBy(x => x).OrderByDescending(x => x.Count()).ToList();
							var points = aaa.Select(x => x.Key);//.Take(numberOfPoints * 2);
							var howManyToAdd = Math.Min(points.Where(x => x.Parent == i).Count(x => !x.Counted), points.Where(x => x.Parent == j).Count(x => !x.Counted));

							aaa.Select(x => x.Key).ForEach(x => x.Counted = true);
							Console.WriteLine($"Adding {howManyToAdd} to common counter");
							count += howManyToAdd;
						}
					}
				}
			}

			var uniquePoints = scanners.SelectMany(x => x.Beacons).Count(x => !x.Counted);
			Console.WriteLine($"Common points {count}");
			Console.WriteLine($"Unique points {uniquePoints}");
			Console.WriteLine($"Result {count + uniquePoints}");
			return count + uniquePoints;
			//scanners.Select(x =>
			//{
			//  for (int i = 0; i < x.Beacons; i++)
			//    for (int j = 0; j < x.Beacons; j++)
			//    {
			//      var p1 = x.Beacons[i];
			//      var p2 = x.Beacons[j];
			//      if (p1 != p2)
			//        pair.Add(Pair.CreatePoint(new Point(i, 0, 0), new Point(j, 0, 0)));
			//    }

			//  x.Beacons.Select(x => )
			//  });

			return pair.Distinct().Count();
		}

		public object? SolvePart2()
		{
			return null;
		}

		private List<Scanner> ParseInput()
		{
			var returnList = new List<Scanner>();
			var currentPointList = new List<Point>();
			var scannerNumber = 0;
			for (int i = 0; i < this.commands.Count(); i++)
			{
				if (string.IsNullOrWhiteSpace(this.commands[i]))
					continue;

				if (this.commands[i].Contains("---"))
				{
					if (currentPointList.Count > 0)
					{
						returnList.Add(new(currentPointList));
						currentPointList = new List<Point>();
						scannerNumber++;
					}

					continue;
				}

				var split = this.commands[i].Split(',').Select(x => int.Parse(x)).ToArray();
				currentPointList.Add(new Point(split[0], split[1], split[2], scannerNumber));
			}

			returnList.Add(new(currentPointList));

			return returnList;
		}
	}
}


