using MoreLinq;

namespace AdventOfCode2023;

internal class GearRatiosSolution : ChallengeBase
{
	public override string Title => "Gear Ratios";

	public override int Day => 3;

	public override object SolvePart1()
	{
		var games = ParseInput();
		return games.GetNumbers().Where(num =>
		{
			//// up row
			if (Enumerable.Range(num.X - 1, num.Value.ToString().Length + 2).Any(x => games.GetPoint(x, num.Y - 1).IsSymbol()))
				return true;

			//// bottom row
			if (Enumerable.Range(num.X - 1, num.Value.ToString().Length + 2).Any(x => games.GetPoint(x, num.Y + 1).IsSymbol()))
				return true;

			//// left and right
			if (games.GetPoint(num.X - 1, num.Y).IsSymbol() || games.GetPoint(num.X + num.Value.ToString().Length, num.Y).IsSymbol())
				return true;
			return false;
		}).Sum(x => x.Value);
	}

	public override object? SolvePart2()
	{
		var board = ParseInput();
		var allNumbers = board.GetNumbers();

		var exitNumber = 0;
		board.GetGear().ForEach(p =>
		{
			var numbers = new List<Number>();
			//// up row
			Enumerable.Range(p.X - 1, 3).Where(x => board.GetPoint(x, p.Y - 1).IsDigit()).ForEach(x => numbers.Add(allNumbers.Where(n => n.Y == p.Y - 1).First(num => num.ContainsPoint(board.GetPoint(x, p.Y - 1)))));

			//// bottom row
			Enumerable.Range(p.X - 1, 3).Where(x => board.GetPoint(x, p.Y + 1).IsDigit()).ForEach(x => numbers.Add(allNumbers.Where(n => n.Y == p.Y + 1).First(num => num.ContainsPoint(board.GetPoint(x, p.Y + 1)))));

			//// left
			if (board.GetPoint(p.X - 1, p.Y).IsDigit())
				numbers.Add(allNumbers.Where(n => n.Y == p.Y).First(num => num.ContainsPoint(board.GetPoint(p.X - 1, p.Y))));

			if (board.GetPoint(p.X + 1, p.Y).IsDigit())
				numbers.Add(allNumbers.Where(n => n.Y == p.Y).First(num => num.ContainsPoint(board.GetPoint(p.X + 1, p.Y))));

			if(numbers.Distinct().Count() == 2)
			{
				exitNumber += (numbers.First().Value * numbers.Last().Value);
			}
		});

		return exitNumber;
	}

	private Board ParseInput()
	{
		var lines = File.ReadAllLines(this.InputPath);
		var points = new List<Point>();
		for (int y = 0; y < lines.Length; y++)
		{
			var line = lines[y];
			for (int x = 0; x < line.Length; x++)
			{
				points.Add(new Point(x, y, line[x]));
			}
		}

		return new Board(points);
	}

	internal record Point(int X, int Y, char Value)
	{
		public bool IsSymbol() => !char.IsDigit(this.Value) && this.Value != '.';

		public bool IsGear() => this.Value == '*';

		public bool IsDigit() => char.IsDigit(this.Value);
	}

	internal record Number(int Value, int X, int Y, IEnumerable<Point> NumberPoints)
	{
		public bool ContainsPoint(Point point) => this.NumberPoints.Contains(point);
	}

	internal record Board(IEnumerable<Point> Points)
	{
		public Point GetPoint(int x, int y) => this.Points.FirstOrDefault(p => p.X == x && p.Y == y) ?? new Point(x, y, '.');

		public IEnumerable<Point> GetGear() => this.Points.Where(x => x.IsGear()).ToList();

		public IEnumerable<Number> GetNumbers()
		{
			var numbers = new List<Number>();
			var processedPoints = new List<Point>();
			foreach (var point in Points)
			{
				if (processedPoints.Contains(point))
					continue;

				if (point.IsDigit())
				{
					List<Point> numberPoints = [point];					
					var numberString = point.Value.ToString();
					var tmpX = point.X + 1;
					while (GetPoint(tmpX, point.Y).IsDigit())
					{
						var anotherPoint = GetPoint(tmpX, point.Y);
						numberString += anotherPoint.Value;
						tmpX++;
						processedPoints.Add(anotherPoint);
						numberPoints.Add(anotherPoint);
					}

					numbers.Add(new Number(int.Parse(numberString), point.X, point.Y, numberPoints));
				}

				processedPoints.Add(point);
			}

			return numbers;
		}

	};
}
